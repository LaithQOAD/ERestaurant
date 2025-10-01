using ERestaurant.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Infrastructure.HelperClass.TenantIsolationIndexingDB
{
    public static class TenantIsolationDbContext
    {
        public static void Apply(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>().HasAlternateKey(e => new { e.TenantId, e.Id });
            modelBuilder.Entity<Combo>().HasAlternateKey(e => new { e.TenantId, e.Id });
            modelBuilder.Entity<AdditionalMaterial>().HasAlternateKey(e => new { e.TenantId, e.Id });
            modelBuilder.Entity<Order>().HasAlternateKey(e => new { e.TenantId, e.Id });

            modelBuilder.Entity<ComboMaterial>()
                .HasOne(cm => cm.Combo)
                .WithMany(c => c.ComboMaterial)
                .HasForeignKey(cm => new { cm.ComboId, cm.TenantId })
                .HasPrincipalKey(c => new { c.Id, c.TenantId })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ComboMaterial>()
                .HasOne(cm => cm.Material)
                .WithMany(m => m.ComboMaterial)
                .HasForeignKey(cm => new { cm.MaterialId, cm.TenantId })
                .HasPrincipalKey(m => new { m.Id, m.TenantId })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItem)
                .HasForeignKey(oi => new { oi.OrderId, oi.TenantId })
                .HasPrincipalKey(o => new { o.Id, o.TenantId })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Material)
                .WithMany()
                .HasForeignKey(oi => new { oi.MaterialId, oi.TenantId })
                .HasPrincipalKey(m => new { m.Id, m.TenantId })
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Combo)
                .WithMany()
                .HasForeignKey(oi => new { oi.ComboId, oi.TenantId })
                .HasPrincipalKey(c => new { c.Id, c.TenantId })
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.AdditionalMaterial)
                .WithMany()
                .HasForeignKey(oi => new { oi.AdditionalMaterialId, oi.TenantId })
                .HasPrincipalKey(a => new { a.Id, a.TenantId })
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Material>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<Combo>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<AdditionalMaterial>().HasIndex(e => e.TenantId);
            modelBuilder.Entity<Order>().HasIndex(e => e.TenantId);

            modelBuilder.Entity<ComboMaterial>().HasIndex(e => new { e.TenantId, e.ComboId });
            modelBuilder.Entity<ComboMaterial>().HasIndex(e => new { e.TenantId, e.MaterialId });

            modelBuilder.Entity<OrderItem>().HasIndex(e => new { e.TenantId, e.OrderId });
            modelBuilder.Entity<OrderItem>().HasIndex(e => new { e.TenantId, e.MaterialId });
            modelBuilder.Entity<OrderItem>().HasIndex(e => new { e.TenantId, e.ComboId });
            modelBuilder.Entity<OrderItem>().HasIndex(e => new { e.TenantId, e.AdditionalMaterialId });
        }
    }
}
