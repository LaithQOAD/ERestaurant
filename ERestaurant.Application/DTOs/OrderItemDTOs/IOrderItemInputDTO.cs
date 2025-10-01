namespace ERestaurant.Application.DTOs.OrderItemDTOs
{
    public interface IOrderItemInputDTO
    {
        Guid? MaterialId { get; }
        Guid? ComboId { get; }
        Guid? AdditionalMaterialId { get; }
        int Quantity { get; }
    }
}
