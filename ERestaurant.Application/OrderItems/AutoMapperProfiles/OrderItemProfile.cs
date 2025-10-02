using AutoMapper;
using ERestaurant.Application.OrderItems.DTOs;
using ERestaurant.Domain.Entity;

namespace ERestaurant.Application.OrderItems.AutoMapperProfiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDTO>();
        }
    }
}
