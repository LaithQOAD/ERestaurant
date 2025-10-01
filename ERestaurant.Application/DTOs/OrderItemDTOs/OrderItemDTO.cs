using ERestaurant.Application.DTOs.AdditionalMaterialDTOs;
using ERestaurant.Application.DTOs.ComboDTOs;
using ERestaurant.Application.DTOs.MaterialDTOs;

namespace ERestaurant.Application.DTOs.OrderItemDTOs
{
    public class OrderItemDTO
    {
        public int Quantity { get; set; }

        public decimal PriceBeforeTax { get; set; }

        public decimal Tax { get; set; }

        public decimal PriceAfterTax { get; set; }

        public Guid? MaterialId { get; set; }
        public virtual MaterialDTO? Material { get; set; }

        public Guid? ComboId { get; set; }
        public virtual ComboDTO? Combo { get; set; }

        public Guid? AdditionalMaterialId { get; set; }
        public virtual AdditionalMaterialDTO? AdditionalMaterial { get; set; }


    }
}
