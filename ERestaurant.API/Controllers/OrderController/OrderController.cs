using ERestaurant.Application.Orders.DTOs;
using ERestaurant.Application.Orders.OrderServices;
using Microsoft.AspNetCore.Mvc;

namespace ERestaurant.API.Controllers.OrderController
{
    [ApiController]
    [Route("API/Order")]
    public sealed class OrderController : ControllerBase
    {
        private readonly IOrderService _orderServices;

        public OrderController(IOrderService orders)
        {
            _orderServices = orders;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> FindAllAsync([FromQuery] FindAllOrderParameterDTO findAllOrderParameterDTO)
        {
            var result = await _orderServices.FindAllAsync(findAllOrderParameterDTO);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDTO>> FindByIdAsync(Guid id)
        {
            var dto = await _orderServices.FindByIdAsync(id);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateAsync([FromBody] CreateOrderDTO newOrder)
        {
            var created = await _orderServices.CreateAsync(newOrder);
            return Created();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<OrderDTO>> UpdateAsync(Guid id, [FromBody] UpdateOrderDTO updatedOrder)
        {
            if (id != updatedOrder.Id)
                return BadRequest("Route id and body id must match.");

            var dto = await _orderServices.UpdateAsync(updatedOrder);
            return Ok(dto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _orderServices.DeleteAsync(id);
            return NoContent();
        }
    }
}
