using ERestaurant.Application.AdditionalMaterials.AdditionalMaterialDTOs;

namespace ERestaurant.Application.DTOs.ComboDTOs
{
    public class ComboWithAdditionalMaterialDTO
    {
        public List<ComboDTO> Combo { get; set; }
        public List<AdditionalMaterialDTO>? AdditionalMaterial { get; set; }

    }
}
