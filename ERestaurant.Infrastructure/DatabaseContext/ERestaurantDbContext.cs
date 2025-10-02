using ERestaurant.Application.Services.MiddlewareInterfaces;
using ERestaurant.Domain.Entity;
using ERestaurant.Infrastructure.HelperClass.Auditing;
using ERestaurant.Infrastructure.HelperClass.AutoIncludeDB;
using ERestaurant.Infrastructure.HelperClass.GlobalFilterDB;
using ERestaurant.Infrastructure.HelperClass.PricingConstraintDB;
using ERestaurant.Infrastructure.HelperClass.SeedData;
using ERestaurant.Infrastructure.HelperClass.TenantIsolationIndexingDB;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Infrastructure.DatabaseContext
{
    public class ERestaurantDbContext : DbContext
    {
        private readonly IRequestTenant _requestTenant;
        private int CurrentTenantId => _requestTenant?.TenantId ?? 0;

        private readonly IAuditStamper _audit;
        public IReadOnlyList<SavedChange> LastSavedChanges { get; private set; } = Array.Empty<SavedChange>();
        public DbSet<Material> Material { get; set; }
        public DbSet<AdditionalMaterial> AdditionalMaterial { get; set; }
        public DbSet<Combo> Combo { get; set; }
        public DbSet<ComboMaterial> ComboMaterial { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }

        public ERestaurantDbContext(
            DbContextOptions<ERestaurantDbContext> options,
            IRequestTenant RequestTenant,
            IAuditStamper audit) :
            base(options)
        {
            _requestTenant = RequestTenant;
            _audit = audit;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyGlobalFilter<BaseEntity>(e => e.TenantId == _requestTenant.TenantId && !e.IsDeleted);

            AutoIncludeDbContext.Apply(modelBuilder);
            
            TenantIsolationDbContext.Apply(modelBuilder);
            
            PricingConstraintDbContext.Apply(modelBuilder);

            SeedData.Apply(modelBuilder);

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var changes = _audit.Apply(ChangeTracker, CurrentTenantId, "System");
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            LastSavedChanges = changes;
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => SaveChangesAsync(true, cancellationToken);

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var changes = _audit.Apply(ChangeTracker, CurrentTenantId, "System");
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            LastSavedChanges = changes;
            return result;
        }
    }
}
