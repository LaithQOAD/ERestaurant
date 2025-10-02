using ERestaurant.Application.Materials.DTOs;

namespace ERestaurant.Application.ComboMaterials.DTOs
{
    public class ComboMaterialDTO
    {
        public int Quantity { get; set; }
        public MaterialDTO? Material { get; set; }
    }
}