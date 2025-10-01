using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERestaurant.Domain.Entity
{
    public class OrderItem : BaseEntity
    {
        [Required]
        public int Quantity { get; set; }

        [Required, Precision(18, 3)]
        public decimal PriceBeforeTax { get; set; }

        [Required, Precision(18, 3)]
        public decimal Tax { get; set; }

        [Required, Precision(18, 3)]
        public decimal PriceAfterTax { get; set; }


        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Material")]
        public Guid? MaterialId { get; set; }
        public virtual Material? Material { get; set; }

        [ForeignKey("Combo")]
        public Guid? ComboId { get; set; }
        public virtual Combo? Combo { get; set; }

        [ForeignKey("AdditionalMaterial")]
        public Guid? AdditionalMaterialId { get; set; }
        public virtual AdditionalMaterial? AdditionalMaterial { get; set; }
    }
}
