using ERestaurant.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.Materials.DTOs
{
    public class CreateMaterialDTO
    {
        [Required, MaxLength(50)]
        public string NameEn { get; set; }

        [Required, MaxLength(50)]
        public string NameAr { get; set; }

        [Required]
        public MaterialUnit Unit { get; set; }

        [Required, Precision(18, 3)]
        public decimal PricePerUnit { get; set; }

        [Required, Precision(18, 3), Range(0, 1)]
        public decimal Tax { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
