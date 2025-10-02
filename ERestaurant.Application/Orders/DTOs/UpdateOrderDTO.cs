using ERestaurant.Application.OrderItems.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.Orders.DTOs
{
    public class UpdateOrderDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string? CustomerName { get; set; }

        [Required, MaxLength(50)]
        public string? CustomerPhone { get; set; }

        [Required]
        public List<UpdateOrderItemDTO> OrderItem { get; set; }

    }
}
