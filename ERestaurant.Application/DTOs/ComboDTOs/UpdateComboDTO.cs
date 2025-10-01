using ERestaurant.Application.DTOs.ComboMaterialDTOs;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.DTOs.ComboDTOs
{
    public class UpdateComboDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string NameEn { get; set; } = default!;

        [Required, MaxLength(50)]
        public string NameAr { get; set; } = default!;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }

        [Range(0, 1)]
        public decimal Tax { get; set; }

        [Required]
        public List<CreateAndUpdateComboMaterialDTO> Material { get; set; }
    }

}
