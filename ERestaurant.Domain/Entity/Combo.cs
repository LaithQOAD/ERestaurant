using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Domain.Entity
{
    public class Combo : BaseEntity
    {
        [Required, MaxLength(50)]
        public string NameEn { get; set; }

        [Required, MaxLength(50)]
        public string NameAr { get; set; }

        [Required, Precision(18, 3), Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required, Precision(18, 3), Range(0, 1)]
        public decimal Tax { get; set; }

        public virtual ICollection<ComboMaterial> ComboMaterial { get; set; }
    }
}
