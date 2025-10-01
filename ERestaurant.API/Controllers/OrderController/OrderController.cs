using ERestaurant.Application.DTOs.OrderDTOs;
using ERestaurant.Application.Services.OrderServices;
using Microsoft.AspNetCore.Mvc;

namespace ERestaurant.API.Controllers.OrderController
{
    [ApiController]
    [Route("API/Order")]
    public sealed class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orders)
        {
            _orderServices = orders;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> FindAllAsync(
            [FromQuery] string? searchQuery,
            [FromQuery] DateTimeOffset? dateFrom,
            [FromQuery] DateTimeOffset? dateTo,
            [FromQuery] bool? isActive,
            [FromQuery] string? orderBy = "OrderDate",
            [FromQuery] string? orderByDirection = "DESC",
            [FromQuery] int? pageNumber = 1,
            [FromQuery] int? pageSize = 10)
        {
            var result = await _orderServices.FindAllAsync(
                searchQuery, dateFrom, dateTo, isActive,
                orderBy, orderByDirection, pageNumber, pageSize);

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
