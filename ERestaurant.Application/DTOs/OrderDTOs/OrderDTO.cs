using ERestaurant.Application.DTOs.OrderItemDTOs;
using ERestaurant.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public bool IsActive { get; set; }

        public decimal TotalPriceBeforeTax { get; set; }

        public decimal TotalTax { get; set; }

        public decimal TotalPriceAfterTax { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public virtual ICollection<OrderItemDTO> OrderItem { get; set; }
    }
}
