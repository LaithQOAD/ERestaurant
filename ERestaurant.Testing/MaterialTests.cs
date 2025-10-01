using ERestaurant.Domain.Entity;
using ERestaurant.Domain.Enums;
using ERestaurant.Infrastructure.DatabaseContext;
using ERestaurant.Infrastructure.HelperClass.Pagination;
using ERestaurant.Testing.SharedHelperClass;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Testing
{
    public sealed class MaterialTests
    {
        private static ERestaurantDbContext CreateContext(int tenantId)
        {
            var options = new DbContextOptionsBuilder<ERestaurantDbContext>()
                .UseInMemoryDatabase(databaseName: $"matdb-{Guid.NewGuid()}")
                .Options;

            return new ERestaurantDbContext(
                options,
                new MockRequestTenant { TenantId = tenantId },
                new NoopAuditStamper());
        }

        private static void SeedMaterials(ERestaurantDbContext ctx, int total = 30)
        {
            var now = DateTimeOffset.UtcNow;
            var list = new List<Material>();

            for (int i = 1; i <= total; i++)
            {
                list.Add(new Material
                {
                    Id = Guid.NewGuid(),
                    TenantId = 1,
                    CreatedBy = "seed",
                    CreatedDate = now,
                    IsDeleted = false,
                    NameEn = $"Mat-{i:000}-EN",
                    NameAr = $"مادة-{i:000}",
                    Unit = (i % 2 == 0) ? MaterialUnit.Piece : MaterialUnit.Gram,
                    PricePerUnit = i,
                    Tax = 0.16m,
                    ImageUrl = null,
                    IsActive = i % 2 == 1
                });
            }

            ctx.Material.AddRange(list);
            ctx.SaveChanges();
        }

        [Fact]
        public async Task FilterByNameContainsWorks()
        {
            using var ctx = CreateContext(tenantId: 1);
            SeedMaterials(ctx, total: 20);

            var s = "Mat-00";
            var q = ctx.Material.AsQueryable();

            q = q.Where(m =>
                EF.Functions.Like(m.NameEn, $"%{s}%") ||
                EF.Functions.Like(m.NameAr, $"%{s}%"));

            var result = await q.ToListAsync();
            Assert.True(result.Count >= 9);
            Assert.All(result, m => Assert.Contains("Mat-00", m.NameEn));
        }

        [Fact]
        public async Task FilterByIsActiveAndUnitWorks()
        {
            using var ctx = CreateContext(tenantId: 1);
            SeedMaterials(ctx, total: 20);

            var q = ctx.Material.AsQueryable();


            q = q.Where(m => m.IsActive);

            q = q.Where(m => m.Unit == MaterialUnit.Gram);

            var list = await q.ToListAsync();

            Assert.NotEmpty(list);
            Assert.All(list, m =>
            {
                Assert.True(m.IsActive);
                Assert.Equal(MaterialUnit.Gram, m.Unit);
            });
        }

        [Fact]
        public async Task OrderingByNameEnDESCWorks()
        {
            using var ctx = CreateContext(tenantId: 1);
            SeedMaterials(ctx, total: 10);

            var q = ctx.Material.OrderByDescending(m => m.NameEn);
            var first = await q.FirstAsync();

            Assert.Equal("Mat-010-EN", first.NameEn);
        }

        [Fact]
        public async Task PagingMaxPageSizeClampsTo15()
        {
            using var ctx = CreateContext(tenantId: 1);
            SeedMaterials(ctx, total: 40);

            var mainQuery = ctx.Material.OrderBy(m => m.CreatedDate);

            var total = await mainQuery.CountAsync();

            var p = PagingHelper.Compute(pageNumber: 1, pageSize: 100, totalCount: total);

            var page = await mainQuery.Skip(p.Skip).Take(p.Take).ToListAsync();

            Assert.Equal(15, p.PageSize);
            Assert.Equal(15, page.Count);
        }
    }
}
