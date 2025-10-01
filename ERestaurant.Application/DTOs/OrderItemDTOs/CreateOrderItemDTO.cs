using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.DTOs.OrderItemDTOs
{
    public class CreateOrderItemDTO : IOrderItemInputDTO
    {
        public Guid? MaterialId { get; set; }
        public Guid? ComboId { get; set; }
        public Guid? AdditionalMaterialId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
