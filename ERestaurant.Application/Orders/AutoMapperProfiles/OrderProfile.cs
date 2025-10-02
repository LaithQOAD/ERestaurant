using AutoMapper;
using ERestaurant.Application.Orders.DTOs;
using ERestaurant.Domain.Entity;

namespace ERestaurant.Application.Orders.AutoMapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>();
        }
    }
}
