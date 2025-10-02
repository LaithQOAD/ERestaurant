
using ERestaurant.Application.ComboMaterials.DTOs;

namespace ERestaurant.Application.DTOs.ComboDTOs
{
    public class ComboDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public decimal Tax { get; set; }

        public ICollection<ComboMaterialDTO> ComboMaterial { get; set; }
    }
}
