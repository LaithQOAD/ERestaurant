using ERestaurant.Application.DTOs.AdditionalMaterialDTOs;
using ERestaurant.Domain.Enums;

namespace ERestaurant.Application.Services.AdditionalMaterialServices
{
    public interface IAdditionalMaterialServices
    {
        // Reads
        Task<List<AdditionalMaterialDTO>> FindAllAsync(
            string? searchQuery = null,
            MaterialUnit? unit = null,
            bool? isActive = null,
            decimal? priceFrom = null,
            decimal? priceTo = null,
            string? orderBy = "Name",
            string? orderByDirection = "ASC",
            int? pageNumber = 1,
            int? pageSize = 10);

        Task<AdditionalMaterialDTO?> FindByIdAsync(Guid id);

        // Writes
        Task<AdditionalMaterialDTO> AddAsync(CreateAdditionalMaterialDTO newAdditionalMaterial);
        Task<AdditionalMaterialDTO> UpdateAsync(UpdateAdditionalMaterialDTO updatedAdditionalMaterial);
        Task DeleteAsync(Guid id);
    }

}
