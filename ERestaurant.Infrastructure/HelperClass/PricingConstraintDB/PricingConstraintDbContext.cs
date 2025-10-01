using ERestaurant.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Infrastructure.HelperClass.PricingConstraintDB
{
    public static class PricingConstraintDbContext
    {
        public static void Apply(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>().ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_Material_PricePerUnit_GT0", "[PricePerUnit] > 0");
                tb.HasCheckConstraint("CK_Material_Tax_0_1", "[Tax] >= 0 AND [Tax] <= 1");
            });

            modelBuilder.Entity<AdditionalMaterial>().ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_AdditionalMaterial_PricePerUnit_GT0", "[PricePerUnit] > 0");
                tb.HasCheckConstraint("CK_AdditionalMaterial_Tax_0_1", "[Tax] >= 0 AND [Tax] <= 1");
            });

            modelBuilder.Entity<Combo>().ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_Combo_Price_GT0", "[Price] > 0");
                tb.HasCheckConstraint("CK_Combo_Tax_0_1", "[Tax] >= 0 AND [Tax] <= 1");
            });

            modelBuilder.Entity<ComboMaterial>().ToTable(tb =>
                tb.HasCheckConstraint("CK_ComboMaterial_Qty_GT0", "[Quantity] > 0"));

            modelBuilder.Entity<OrderItem>().ToTable(tb =>
                tb.HasCheckConstraint("CK_OrderItem_Qty_GT0", "[Quantity] > 0"));
        }
    }
}
