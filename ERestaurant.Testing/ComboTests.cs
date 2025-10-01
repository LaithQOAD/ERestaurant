using ERestaurant.Application.Services.Middleware.Interfaces;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.Enums;
using ERestaurant.Infrastructure.DatabaseContext;
using ERestaurant.Infrastructure.HelperClass.Auditing;
using ERestaurant.Testing.SharedHelperClass;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Testing
{
    public sealed class ComboTests
    {
        private static ERestaurantDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ERestaurantDbContext>()
                .UseInMemoryDatabase($"combodb-{Guid.NewGuid()}")
                .Options;

            return new ERestaurantDbContext(options, new MockRequestTenant { TenantId = 1 }, new NoopAuditStamper());
        }

        [Fact]
        public async Task ComboShouldIncludeMaterialsWithQuantities()
        {
            using var ctx = CreateContext();

            var m1 = NewMat("Flour", 1m);
            var m2 = NewMat("Cheese", 1m);
            var m3 = NewMat("Tomato", 1m);

            ctx.Material.AddRange(m1, m2, m3);

            var combo = new Combo
            {
                Id = Guid.NewGuid(),
                TenantId = 1,
                CreatedBy = "seed",
                CreatedDate = DateTimeOffset.UtcNow,
                NameEn = "Test Combo",
                NameAr = "كومبو اختبار",
                Price = 5m,
                IsActive = true,
                Tax = 0.16m
            };

            ctx.Combo.Add(combo);

            ctx.ComboMaterial.AddRange(
                new ComboMaterial { Id = Guid.NewGuid(), TenantId = 1, CreatedBy = "seed", CreatedDate = DateTimeOffset.UtcNow, ComboId = combo.Id, MaterialId = m1.Id, Quantity = 1 },
                new ComboMaterial { Id = Guid.NewGuid(), TenantId = 1, CreatedBy = "seed", CreatedDate = DateTimeOffset.UtcNow, ComboId = combo.Id, MaterialId = m2.Id, Quantity = 2 },
                new ComboMaterial { Id = Guid.NewGuid(), TenantId = 1, CreatedBy = "seed", CreatedDate = DateTimeOffset.UtcNow, ComboId = combo.Id, MaterialId = m3.Id, Quantity = 3 }
            );

            await ctx.SaveChangesAsync();

            var fetched = await ctx.Combo
                .Include(c => c.ComboMaterial)
                .ThenInclude(cm => cm.Material)
                .SingleAsync(c => c.Id == combo.Id);

            Assert.Equal(3, fetched.ComboMaterial.Count);

            var ordered = fetched.ComboMaterial.OrderBy(x => x.Material!.NameEn).ToList();
            Assert.Equal("Cheese", ordered[0].Material!.NameEn);
            Assert.Equal(2, ordered[0].Quantity);

            Assert.Equal("Flour", ordered[1].Material!.NameEn);
            Assert.Equal(1, ordered[1].Quantity);

            Assert.Equal("Tomato", ordered[2].Material!.NameEn);
            Assert.Equal(3, ordered[2].Quantity);
        }

        [Fact]
        public async Task ComboTotal_UsingComboPrice_WithTax_IsCorrect()
        {
            using var ctx = CreateContext();

            var m1 = NewMat("Flour", 1m);
            var m2 = NewMat("Cheese", 1m);
            ctx.Material.AddRange(m1, m2);

            var combo = new Combo
            {
                Id = Guid.NewGuid(),
                TenantId = 1,
                CreatedBy = "seed",
                CreatedDate = DateTimeOffset.UtcNow,
                NameEn = "Test Combo",
                NameAr = "كومبو اختبار",
                Price = 5m,
                Tax = 0.16m,
                IsActive = true
            };
            ctx.Combo.Add(combo);

            ctx.ComboMaterial.AddRange(
                new ComboMaterial { Id = Guid.NewGuid(), TenantId = 1, CreatedBy = "seed", CreatedDate = DateTimeOffset.UtcNow, ComboId = combo.Id, MaterialId = m1.Id, Quantity = 1 },
                new ComboMaterial { Id = Guid.NewGuid(), TenantId = 1, CreatedBy = "seed", CreatedDate = DateTimeOffset.UtcNow, ComboId = combo.Id, MaterialId = m2.Id, Quantity = 2 }
            );

            await ctx.SaveChangesAsync();

            var qty = 2;
            var (beforeTax, tax, afterTax) = CalcTotalFromComboPrice(combo, qty);

            Assert.Equal(10.000m, beforeTax);
            Assert.Equal(1.600m, tax);
            Assert.Equal(11.600m, afterTax);
        }

        [Fact]
        public async Task ComboTotal_FromMaterials_WithTax_IsCorrect()
        {
            using var ctx = CreateContext();

            var m1 = NewMat("Flour", 1m);
            var m2 = NewMat("Cheese", 1m);
            var m3 = NewMat("Tomato", 1m);
            ctx.Material.AddRange(m1, m2, m3);

            var combo = new Combo
            {
                Id = Guid.NewGuid(),
                TenantId = 1,
                CreatedBy = "seed",
                CreatedDate = DateTimeOffset.UtcNow,
                NameEn = "Test Combo",
                NameAr = "كومبو اختبار",
                Price = 5m,
                Tax = 0.16m,
                IsActive = true
            };
            ctx.Combo.Add(combo);

            ctx.ComboMaterial.AddRange(
                new ComboMaterial { Id = Guid.NewGuid(), TenantId = 1, CreatedBy = "seed", CreatedDate = DateTimeOffset.UtcNow, ComboId = combo.Id, MaterialId = m1.Id, Quantity = 1 }, // 1 * 1 = 1
                new ComboMaterial { Id = Guid.NewGuid(), TenantId = 1, CreatedBy = "seed", CreatedDate = DateTimeOffset.UtcNow, ComboId = combo.Id, MaterialId = m2.Id, Quantity = 2 }, // 2 * 1 = 2
                new ComboMaterial { Id = Guid.NewGuid(), TenantId = 1, CreatedBy = "seed", CreatedDate = DateTimeOffset.UtcNow, ComboId = combo.Id, MaterialId = m3.Id, Quantity = 3 }  // 3 * 1 = 3
            );

            await ctx.SaveChangesAsync();

            var fetched = await ctx.Combo
                .Include(c => c.ComboMaterial)
                .ThenInclude(cm => cm.Material)
                .SingleAsync(c => c.Id == combo.Id);

            var qty = 2;
            var (beforeTax, tax, afterTax) = CalcTotalFromMaterials(fetched, qty);

            Assert.Equal(12.000m, beforeTax);
            Assert.Equal(1.920m, tax);
            Assert.Equal(13.920m, afterTax);
        }

        private static (decimal beforeTax, decimal tax, decimal afterTax) CalcTotalFromComboPrice(Combo combo, int quantity)
        {
            var before = combo.Price * quantity;
            var tax = Math.Round(before * combo.Tax, 3, MidpointRounding.AwayFromZero);
            var after = before + tax;
            return (before, tax, after);
        }

        private static (decimal beforeTax, decimal tax, decimal afterTax) CalcTotalFromMaterials(Combo combo, int quantity)
        {
            if (combo.ComboMaterial is null || combo.ComboMaterial.Count == 0)
                return (0m, 0m, 0m);

            var perUnitCost = combo.ComboMaterial.Sum(cm =>
                (cm.Material?.PricePerUnit ?? 0m) * cm.Quantity);

            var before = perUnitCost * quantity;
            var tax = Math.Round(before * combo.Tax, 3, MidpointRounding.AwayFromZero);
            var after = before + tax;
            return (before, tax, after);
        }

        private static Material NewMat(string en, decimal pricePerUnit)
            => new Material
            {
                Id = Guid.NewGuid(),
                TenantId = 1,
                CreatedBy = "seed",
                CreatedDate = DateTimeOffset.UtcNow,
                NameEn = en,
                NameAr = $"AR-{en}",
                Unit = MaterialUnit.Gram,
                PricePerUnit = pricePerUnit,
                Tax = 0.16m,
                IsActive = true
            };
    }
}
