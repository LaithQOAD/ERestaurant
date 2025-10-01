using ERestaurant.Domain.Enums;

namespace ERestaurant.Application.DTOs.AdditionalMaterialDTOs
{
    public class AdditionalMaterialDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public MaterialUnit Unit { get; set; }

        public decimal PricePerUnit { get; set; }

        public decimal Tax { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }
    }
}
