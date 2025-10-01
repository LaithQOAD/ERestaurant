using AutoMapper;
using ERestaurant.Application.DTOs.ComboMaterialDTOs;
using ERestaurant.Domain.Entity;

namespace ERestaurant.Application.Services.AutoMapperProfiles
{
    public class ComboMaterialProfile : Profile
    {
        public ComboMaterialProfile()
        {
            CreateMap<ComboMaterial, ComboMaterialDTO>();
        }
    }

}
