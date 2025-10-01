using AutoMapper;
using ERestaurant.Application.DTOs.OrderDTOs;
using ERestaurant.Domain.Entity;

namespace ERestaurant.Application.Services.AutoMapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>();
        }
    }
}
