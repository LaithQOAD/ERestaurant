using ERestaurant.Application.DTOs.OrderDTOs;

namespace ERestaurant.Application.Services.OrderItemServices
{
    public interface IOrderItemServices
    {
        Task<OrderDTO> AddMaterialAsync(Guid orderId, Guid materialId, int quantity);
        Task<OrderDTO> AddComboAsync(Guid orderId, Guid comboId, int quantity);
        Task<OrderDTO> AddAdditionalMaterialAsync(Guid orderId, Guid additionalMaterialId, int quantity);

        Task<OrderDTO> RemoveAsync(Guid orderId, Guid referenceId);
    }
}
