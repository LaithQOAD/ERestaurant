using ERestaurant.Application.OrderItems.OrderItemServices;
using ERestaurant.Application.Orders.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ERestaurant.API.Controllers
{
    [ApiController]
    [Route("API/Order/{orderId:guid}/Item")]
    public sealed class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemServices _orderItemServices;

        public OrderItemsController(IOrderItemServices orderItems)
        {
            _orderItemServices = orderItems;
        }

        [HttpPost("Material/{materialId:guid}")]
        public async Task<ActionResult<OrderDTO>> AddMaterialAsync(
            Guid orderId,
            Guid materialId,
            [FromQuery] int quantity = 1)
        {
            var updatedOrder = await _orderItemServices.AddMaterialAsync(orderId, materialId, quantity);
            return Ok(updatedOrder);
        }

        [HttpPost("Combo/{comboId:guid}")]
        public async Task<ActionResult<OrderDTO>> AddComboAsync(
            Guid orderId,
            Guid comboId,
            [FromQuery] int quantity = 1)
        {
            var updatedOrder = await _orderItemServices.AddComboAsync(orderId, comboId, quantity);
            return Ok(updatedOrder);
        }

        [HttpPost("Additional/{additionalMaterialId:guid}")]
        public async Task<ActionResult<OrderDTO>> AddAdditionalMaterialAsync(
            Guid orderId,
            Guid additionalMaterialId,
            [FromQuery] int quantity = 1)
        {
            var updatedOrder = await _orderItemServices.AddAdditionalMaterialAsync(orderId, additionalMaterialId, quantity);
            return Ok(updatedOrder);
        }

        [HttpDelete("OrderItem/{referenceId:guid}")]
        public async Task<ActionResult<OrderDTO>> RemoveByReference(Guid orderId, Guid referenceId)
        {
            var updatedOrder = await _orderItemServices.RemoveAsync(orderId, referenceId);
            return Ok(updatedOrder);
        }
    }
}
