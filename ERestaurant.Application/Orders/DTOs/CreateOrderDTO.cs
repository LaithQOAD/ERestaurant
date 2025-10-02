using ERestaurant.Application.OrderItems.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.Orders.DTOs
{
    public class CreateOrderDTO
    {
        [Required, MaxLength(50)]
        public string CustomerName { get; set; }

        [Required, MaxLength(50)]
        public string CustomerPhone { get; set; }

        [Required]
        public virtual ICollection<CreateOrderItemDTO> OrderItem { get; set; }
    }
}
