using Microsoft.EntityFrameworkCore;
using ERestaurant.Infrastructure.DatabaseContext;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.Enums;
using ERestaurant.Testing.SharedHelperClass;

namespace ERestaurant.Testing
{
    public class GlobalFiltersTests
    {
        private static ERestaurantDbContext CreateContext(int tenantId)
        {
            var options = new DbContextOptionsBuilder<ERestaurantDbContext>()
                .UseInMemoryDatabase($"db-{Guid.NewGuid()}")
                .Options;

            var reqTenant = new MockRequestTenant { TenantId = tenantId };
            var audit = new NoopAuditStamper();

            return new ERestaurantDbContext(options, reqTenant, audit);
        }

        [Fact]
        public void QueryShouldReturnOnlyCurrentTenantAndNotDeleted()
        {
            using var ctx = CreateContext(tenantId: 1);

            var now = DateTimeOffset.UtcNow;

            ctx.Material.AddRange(
                new Material
                {
                    Id = Guid.NewGuid(),
                    TenantId = 1,
                    CreatedBy = "t",
                    CreatedDate = now,
                    IsDeleted = false,
                    NameEn = "Flour",
                    NameAr = "طحين",
                    Unit = MaterialUnit.Gram,
                    PricePerUnit = 1.0m,
                    Tax = 0.16m,
                    IsActive = true
                },
                new Material
                {
                    Id = Guid.NewGuid(),
                    TenantId = 2,
                    CreatedBy = "t",
                    CreatedDate = now,
                    IsDeleted = false,
                    NameEn = "Sugar",
                    NameAr = "سكر",
                    Unit = MaterialUnit.Gram,
                    PricePerUnit = 1.0m,
                    Tax = 0.16m,
                    IsActive = true
                },
                new Material
                {
                    Id = Guid.NewGuid(),
                    TenantId = 1,
                    CreatedBy = "t",
                    CreatedDate = now,
                    IsDeleted = true,
                    NameEn = "Butter",
                    NameAr = "زبدة",
                    Unit = MaterialUnit.Gram,
                    PricePerUnit = 1.0m,
                    Tax = 0.16m,
                    IsActive = true
                }
            );

            ctx.SaveChanges();

            var result = ctx.Material.ToList();

            Assert.Single(result);
            Assert.Equal(1, result[0].TenantId);
            Assert.False(result[0].IsDeleted);
            Assert.Equal("Flour", result[0].NameEn);
        }

        [Fact]
        public void QueryShouldChangeOutputWhenTenantChanges()
        {
            var now = DateTimeOffset.UtcNow;

            using (var ctx1 = CreateContext(tenantId: 1))
            {
                ctx1.Material.AddRange(
                    new Material
                    {
                        Id = Guid.NewGuid(),
                        TenantId = 1,
                        CreatedBy = "t",
                        CreatedDate = now,
                        IsDeleted = false,
                        NameEn = "A1",
                        NameAr = "A1",
                        Unit = MaterialUnit.Gram,
                        PricePerUnit = 1m,
                        Tax = 0.16m,
                        IsActive = true
                    },
                    new Material
                    {
                        Id = Guid.NewGuid(),
                        TenantId = 2,
                        CreatedBy = "t",
                        CreatedDate = now,
                        IsDeleted = false,
                        NameEn = "B2",
                        NameAr = "B2",
                        Unit = MaterialUnit.Gram,
                        PricePerUnit = 1m,
                        Tax = 0.16m,
                        IsActive = true
                    }
                );
                ctx1.SaveChanges();

                var list1 = ctx1.Material.ToList();
                Assert.Single(list1);
                Assert.Equal(1, list1[0].TenantId);
                Assert.Equal("A1", list1[0].NameEn);
            }

            using (var ctx2 = CreateContext(tenantId: 2))
            {
                ctx2.Material.AddRange(
                    new Material
                    {
                        Id = Guid.NewGuid(),
                        TenantId = 1,
                        CreatedBy = "t",
                        CreatedDate = now,
                        IsDeleted = false,
                        NameEn = "A1",
                        NameAr = "A1",
                        Unit = MaterialUnit.Gram,
                        PricePerUnit = 1m,
                        Tax = 0.16m,
                        IsActive = true
                    },
                    new Material
                    {
                        Id = Guid.NewGuid(),
                        TenantId = 2,
                        CreatedBy = "t",
                        CreatedDate = now,
                        IsDeleted = false,
                        NameEn = "B2",
                        NameAr = "B2",
                        Unit = MaterialUnit.Gram,
                        PricePerUnit = 1m,
                        Tax = 0.16m,
                        IsActive = true
                    }
                );
                ctx2.SaveChanges();

                var list2 = ctx2.Material.ToList();
                Assert.Single(list2);
                Assert.Equal(2, list2[0].TenantId);
                Assert.Equal("B2", list2[0].NameEn);
            }
        }
    }
}
