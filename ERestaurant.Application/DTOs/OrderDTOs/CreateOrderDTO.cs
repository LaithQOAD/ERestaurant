using ERestaurant.Application.DTOs.OrderItemDTOs;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.DTOs.OrderDTOs
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
