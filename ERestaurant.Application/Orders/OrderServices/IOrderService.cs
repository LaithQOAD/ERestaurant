using ERestaurant.Application.Orders.DTOs;

namespace ERestaurant.Application.Orders.OrderServices
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> FindAllAsync(FindAllOrderParameterDTO findAllOrderParameterDTO);

        Task<OrderDTO?> FindByIdAsync(Guid id);
        Task<OrderDTO> CreateAsync(CreateOrderDTO dto);
        Task<OrderDTO?> UpdateAsync(UpdateOrderDTO dto);
        Task DeleteAsync(Guid id);

    }
}
