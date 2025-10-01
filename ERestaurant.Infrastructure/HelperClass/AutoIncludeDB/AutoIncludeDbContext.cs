using ERestaurant.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Infrastructure.HelperClass.AutoIncludeDB
{
    public static class AutoIncludeDbContext
    {
        public static void Apply(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Combo>().Navigation(x => x.ComboMaterial).AutoInclude();

            modelBuilder.Entity<ComboMaterial>().Navigation(x => x.Material).AutoInclude();

            modelBuilder.Entity<Order>().Navigation(x => x.OrderItem).AutoInclude();

            modelBuilder.Entity<OrderItem>().Navigation(x => x.Material).AutoInclude();
            modelBuilder.Entity<OrderItem>().Navigation(x => x.Combo).AutoInclude();
            modelBuilder.Entity<OrderItem>().Navigation(x => x.AdditionalMaterial).AutoInclude();
        }
    }
}
