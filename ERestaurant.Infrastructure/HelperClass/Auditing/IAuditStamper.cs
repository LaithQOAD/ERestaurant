using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ERestaurant.Infrastructure.HelperClass.Auditing
{
    public interface IAuditStamper
    {
        List<SavedChange> Apply(ChangeTracker tracker, int tenantId, string? userName = "System");
    }
}
