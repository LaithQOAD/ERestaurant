using System.ComponentModel.DataAnnotations.Schema;

namespace ERestaurant.Domain.Entity
{
    public class ComboMaterial : BaseEntity
    {
        public int Quantity { get; set; }

        [ForeignKey("Combo")]
        public Guid ComboId { get; set; }
        public virtual Combo Combo { get; set; }

        [ForeignKey("Material")]
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }

    }
}