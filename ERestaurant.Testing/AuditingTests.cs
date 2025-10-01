using ERestaurant.Application.Services.Middleware.Interfaces;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.Enums;
using ERestaurant.Infrastructure.DatabaseContext;
using ERestaurant.Infrastructure.HelperClass.Auditing;
using ERestaurant.Testing.SharedHelperClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERestaurant.Testing
{
    public sealed class AuditingTests
    {
        private static ERestaurantDbContext CreateContext(int tenantId, string user = "System", string? dbName = null)
        {
            dbName ??= $"audit-db-{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<ERestaurantDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var reqTenant = new MockRequestTenant { TenantId = tenantId };
            var stamper = new TestAuditStamper(user);

            return new ERestaurantDbContext(options, reqTenant, stamper);
        }

        private static Material NewMaterial()
            => new Material
            {
                Id = Guid.NewGuid(),
                NameEn = "Flour",
                NameAr = "طحين",
                Unit = MaterialUnit.Gram,
                PricePerUnit = 1.000m,
                Tax = 0.160m,
                IsActive = true
            };

        [Fact]
        public void CreateShouldSetTenantAndCreatedAudit()
        {
            using var ctx = CreateContext(tenantId: 7, user: "Tester");

            var before = DateTimeOffset.UtcNow;
            ctx.Material.Add(NewMaterial());
            ctx.SaveChanges();
            var after = DateTimeOffset.UtcNow;

            var m = ctx.Material.IgnoreQueryFilters().Single();

            Assert.Equal(7, m.TenantId);
            Assert.Equal("Tester", m.CreatedBy);
            Assert.InRange(m.CreatedDate, before, after);

            Assert.Null(m.UpdatedBy);
            Assert.Null(m.UpdatedDate);
            Assert.False(m.IsDeleted);
            Assert.Null(m.DeletedBy);
            Assert.Null(m.DeletedDate);
        }

        [Fact]
        public void UpdateShouldSetUpdatedAudit()
        {
            var db = $"audit-db-{Guid.NewGuid()}";

            using var ctx = CreateContext(tenantId: 3, user: "Creator", db);
            var m = NewMaterial();
            ctx.Material.Add(m);
            ctx.SaveChanges();
            var createdAt = m.CreatedDate;

            m.NameEn = "Fine Flour";
            ctx.Material.Update(m);
            ctx.SaveChanges();

            using var ctx2 = CreateContext(tenantId: 3, user: "Updater", db);
            var tracked = ctx2.Material.IgnoreQueryFilters().Single(x => x.Id == m.Id);
            tracked.PricePerUnit += 0.100m;
            ctx2.SaveChanges();

            var reloaded = ctx2.Material.IgnoreQueryFilters().Single(x => x.Id == m.Id);
            Assert.Equal("Updater", reloaded.UpdatedBy);
            Assert.NotNull(reloaded.UpdatedDate);
            Assert.True(reloaded.UpdatedDate!.Value >= createdAt);
        }


        [Fact]
        public void SoftDeleteShouldSetDeletedAuditAndBeFilteredOut()
        {
            using var ctx = CreateContext(tenantId: 1, user: "Ops");
            var m = NewMaterial();
            ctx.Material.Add(m);
            ctx.SaveChanges();

            ctx.Material.Remove(m);
            ctx.SaveChanges();

            Assert.Empty(ctx.Material.ToList());

            var only = ctx.Material.IgnoreQueryFilters().Single();
            Assert.True(only.IsDeleted);
            Assert.Equal("Ops", only.DeletedBy);
            Assert.NotNull(only.DeletedDate);
        }
    }

    internal sealed class TestAuditStamper : IAuditStamper
    {
        private readonly string _user;
        public TestAuditStamper(string user)
            => _user = string.IsNullOrWhiteSpace(user) ? "System" : user;

        public List<SavedChange> Apply(ChangeTracker tracker, int tenantId, string? _ = "System")
        {
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in tracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.TenantId = tenantId;
                    entry.Entity.CreatedBy = _user;
                    entry.Entity.CreatedDate = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedBy = _user;
                    entry.Entity.UpdatedDate = now;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedBy = _user;
                    entry.Entity.DeletedDate = now;
                }
            }

            return new();
        }
    }

}
