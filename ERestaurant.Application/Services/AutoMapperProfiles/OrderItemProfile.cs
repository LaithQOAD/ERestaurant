using AutoMapper;
using ERestaurant.Application.DTOs.OrderItemDTOs;
using ERestaurant.Domain.Entity;

namespace ERestaurant.Application.Services.AutoMapperProfiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDTO>();
        }
    }
}
