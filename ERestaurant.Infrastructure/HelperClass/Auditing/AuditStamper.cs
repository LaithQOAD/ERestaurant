using ERestaurant.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ERestaurant.Infrastructure.HelperClass.Auditing
{
    public sealed class AuditStamper : IAuditStamper
    {
        public List<SavedChange> Apply(ChangeTracker tracker, int tenantId, string? userName = "System")
        {
            var now = DateTimeOffset.UtcNow;
            var user = string.IsNullOrWhiteSpace(userName) ? "System" : userName!;
            var list = new List<SavedChange>();

            foreach (var e in tracker.Entries<BaseEntity>())
            {
                if (e.State is EntityState.Detached or EntityState.Unchanged) continue;

                if (e.State == EntityState.Modified)
                {
                    e.Property(x => x.CreatedBy).IsModified = false;
                    e.Property(x => x.CreatedDate).IsModified = false;
                    e.Property(x => x.TenantId).IsModified = false;
                }

                switch (e.State)
                {
                    case EntityState.Added:
                    e.Entity.CreatedBy = string.IsNullOrWhiteSpace(e.Entity.CreatedBy) ? user : e.Entity.CreatedBy;
                    e.Entity.CreatedDate = e.Entity.CreatedDate == default ? now : e.Entity.CreatedDate;
                    if (e.Entity.TenantId == 0 && tenantId != 0) e.Entity.TenantId = tenantId;

                    e.Entity.IsDeleted = false;
                    e.Entity.DeletedBy = null;
                    e.Entity.DeletedDate = null;
                    e.Entity.UpdatedBy = null;
                    e.Entity.UpdatedDate = null;

                    list.Add(new SavedChange(e.Entity.GetType().Name, e.Entity.Id, "Added"));
                    break;

                    case EntityState.Modified:
                    e.Entity.UpdatedBy = user;
                    e.Entity.UpdatedDate = now;

                    var wasDeleted = e.OriginalValues.GetValue<bool>(nameof(BaseEntity.IsDeleted));
                    var isDeleted = e.CurrentValues.GetValue<bool>(nameof(BaseEntity.IsDeleted));
                    if (!wasDeleted && isDeleted)
                    {
                        e.Entity.DeletedBy = user;
                        e.Entity.DeletedDate = now;
                        list.Add(new SavedChange(e.Entity.GetType().Name, e.Entity.Id, "SoftDeleted"));
                    }
                    else
                    {
                        list.Add(new SavedChange(e.Entity.GetType().Name, e.Entity.Id, "Updated"));
                    }
                    break;

                    case EntityState.Deleted:
                    e.State = EntityState.Modified;
                    e.Entity.IsDeleted = true;
                    e.Entity.DeletedBy = user;
                    e.Entity.DeletedDate = now;
                    e.Entity.UpdatedBy = user;
                    e.Entity.UpdatedDate = now;

                    e.Property(x => x.CreatedBy).IsModified = false;
                    e.Property(x => x.CreatedDate).IsModified = false;
                    e.Property(x => x.TenantId).IsModified = false;

                    list.Add(new SavedChange(e.Entity.GetType().Name, e.Entity.Id, "SoftDeleted"));
                    break;
                }
            }

            return list;
        }
    }

}
