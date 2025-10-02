using ERestaurant.Application.AdditionalMaterials.AdditionalMaterialDTOs;
using ERestaurant.Application.AdditionalMaterials.DTOs;
using ERestaurant.Domain.Enums;

namespace ERestaurant.Application.AdditionalMaterials.AdditionalMaterialServices
{
    public interface IAdditionalMaterialService
    {
        // Reads
        Task<List<AdditionalMaterialDTO>> FindAllAsync(
            FindAllAdditionalMaterialParameterDTO findAllAdditionalMaterialParameterDTO);

        Task<AdditionalMaterialDTO?> FindByIdAsync(Guid id);

        // Writes
        Task<AdditionalMaterialDTO> AddAsync(CreateAdditionalMaterialDTO newAdditionalMaterial);
        Task<AdditionalMaterialDTO> UpdateAsync(UpdateAdditionalMaterialDTO updatedAdditionalMaterial);
        Task DeleteAsync(Guid id);
    }

}
