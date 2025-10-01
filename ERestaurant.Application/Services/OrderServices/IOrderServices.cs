using ERestaurant.Application.DTOs.OrderDTOs;

namespace ERestaurant.Application.Services.OrderServices
{
    public interface IOrderServices
    {
        Task<List<OrderDTO>> FindAllAsync(
            string? searchQuery = null,
            DateTimeOffset? dateFrom = null,
            DateTimeOffset? dateTo = null,
            bool? isActive = null,
            string? orderBy = "OrderDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10);

        Task<OrderDTO?> FindByIdAsync(Guid id);
        Task<OrderDTO> CreateAsync(CreateOrderDTO dto);
        Task<OrderDTO?> UpdateAsync(UpdateOrderDTO dto);
        Task DeleteAsync(Guid id);

    }
}
