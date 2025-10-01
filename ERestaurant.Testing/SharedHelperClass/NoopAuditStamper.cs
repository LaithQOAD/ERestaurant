using ERestaurant.Domain.Entity;
using ERestaurant.Infrastructure.HelperClass.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ERestaurant.Testing.SharedHelperClass
{
    internal sealed class NoopAuditStamper : IAuditStamper
    {
        private readonly string _user;
        public NoopAuditStamper(string? userName = "") =>
            _user = string.IsNullOrWhiteSpace(userName) ? "System" : userName;

        public List<SavedChange> Apply(ChangeTracker tracker, int tenantId, string? __ = "System")
        {
            var now = DateTimeOffset.UtcNow;

            foreach (var e in tracker.Entries<BaseEntity>())
            {
                switch (e.State)
                {
                    case EntityState.Added:
                    if (e.Entity.TenantId == 0)
                        e.Entity.TenantId = tenantId;

                    if (string.IsNullOrWhiteSpace(e.Entity.CreatedBy))
                        e.Entity.CreatedBy = _user;

                    if (e.Entity.CreatedDate == default)
                        e.Entity.CreatedDate = now;

                    break;

                    case EntityState.Modified:
                    e.Entity.UpdatedBy = _user;
                    e.Entity.UpdatedDate = now;
                    break;

                    case EntityState.Deleted:
                    break;
                }
            }
            return new();
        }
    }
}
