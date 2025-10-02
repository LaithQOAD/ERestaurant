using ERestaurant.Application.DTOs.ComboDTOs;

namespace ERestaurant.Application.ComboMaterials.ComboMaterialServices
{
    public interface IComboMaterialService
    {
        Task<ComboDTO> AddMaterialAsync(Guid comboId, Guid materialId, int quantity);
        Task<ComboDTO> UpdateQuantityAsync(Guid comboId, Guid materialId, int newQuantity);
        Task<ComboDTO> RemoveMaterialAsync(Guid comboId, Guid materialId);
    }
}
