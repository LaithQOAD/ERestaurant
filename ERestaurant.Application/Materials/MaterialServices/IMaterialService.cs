using ERestaurant.Application.Materials.DTOs;
using ERestaurant.Domain.Enums;

namespace ERestaurant.Application.Materials.MaterialServices
{
    public interface IMaterialService
    {
        Task<List<MaterialDTO>> FindAllAsync(FindAllMaterialParameterDTO findAllMaterialParameterDTO);

        Task<MaterialDTO?> FindByIdAsync(Guid id);
        Task<MaterialDTO> CreateAsync(CreateMaterialDTO dto);
        Task<MaterialDTO> UpdateAsync(UpdateMaterialDTO dto);
        Task DeleteAsync(Guid id);
    }
}
