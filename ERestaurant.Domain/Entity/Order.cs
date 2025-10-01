using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Domain.Entity
{
    public class Order : BaseEntity
    {
        [Required, MaxLength(50)]
        public string CustomerName { get; set; }

        [Required, MaxLength(50)]
        public string CustomerPhone { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required, Precision(18, 3)]
        public decimal TotalPriceBeforeTax { get; set; }

        [Required, Precision(18, 3)]
        public decimal TotalTax { get; set; }

        [Required, Precision(18, 3)]
        public decimal TotalPriceAfterTax { get; set; }

        [Required]
        public DateTimeOffset OrderDate { get; set; }

        [Required]
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
