using ERestaurant.Application.Combos.DTOs;
using ERestaurant.Application.DTOs.ComboDTOs;

namespace ERestaurant.Application.Combos.ComboServices
{
    public interface IComboService
    {
        public Task<ComboWithAdditionalMaterialDTO> FindAllWithAdditionsAsync(FindAllComboParameterDTO findAllParameter);

        Task<List<ComboDTO>> FindAllAsync(FindAllComboParameterDTO findAllParameter);

        Task<ComboDTO?> FindByIdAsync(Guid id);
        Task<ComboDTO> CreateAsync(CreateComboDTO dto);
        Task<ComboDTO> UpdateAsync(UpdateComboDTO dto);
        Task DeleteAsync(Guid id);
    }
}
