using ERestaurant.Application.ComboMaterials.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.DTOs.ComboDTOs
{
    public class CreateComboDTO
    {
        [Required, MaxLength(50)]
        public string NameEn { get; set; } = default!;

        [Required, MaxLength(50)]
        public string NameAr { get; set; } = default!;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(0, 1)]
        public decimal Tax { get; set; }

        [Required]
        public List<CreateAndUpdateComboMaterialDTO> Material { get; set; }
    }

}
