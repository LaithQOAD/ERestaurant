using ERestaurant.Application.DTOs.ComboDTOs;

namespace ERestaurant.Application.Services.ComboServices
{
    public interface IComboServices
    {
        public Task<ComboWithAdditionalMaterialDTO> FindAllWithAdditionsAsync(
            string? searchComboNameQuery = null,
            string? filterComboNameQuery = null,
            bool? isActive = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? orderBy = "CreatedDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10);

        Task<List<ComboDTO>> FindAllAsync(
            string? searchComboNameQuery = null,
            string? filterComboNameQuery = null,
            bool? isActive = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? orderBy = "CreatedDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10
            );

        Task<ComboDTO?> FindByIdAsync(Guid id);
        Task<ComboDTO> CreateAsync(CreateComboDTO dto);
        Task<ComboDTO> UpdateAsync(UpdateComboDTO dto);
        Task DeleteAsync(Guid id);
    }
}
