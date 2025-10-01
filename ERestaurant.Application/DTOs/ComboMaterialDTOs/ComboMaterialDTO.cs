using ERestaurant.Application.DTOs.MaterialDTOs;

namespace ERestaurant.Application.DTOs.ComboMaterialDTOs
{
    public class ComboMaterialDTO
    {
        public int Quantity { get; set; }
        public MaterialDTO? Material { get; set; }
    }
}