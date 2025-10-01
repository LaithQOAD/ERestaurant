using ERestaurant.Application.DTOs.MaterialDTOs;
using ERestaurant.Domain.Enums;

namespace ERestaurant.Application.Services.MaterialServices
{
    public interface IMaterialServices
    {
        Task<List<MaterialDTO>> FindAllAsync(
            string? searchNameQuery = null,
            bool? isActive = null,
            MaterialUnit? unit = null,
            string? orderBy = "CreatedDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10);

        Task<MaterialDTO?> FindByIdAsync(Guid id);
        Task<MaterialDTO> CreateAsync(CreateMaterialDTO dto);
        Task<MaterialDTO> UpdateAsync(UpdateMaterialDTO dto);
        Task DeleteAsync(Guid id);
    }
}
