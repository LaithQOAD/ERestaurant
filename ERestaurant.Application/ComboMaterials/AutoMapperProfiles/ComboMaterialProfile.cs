using AutoMapper;
using ERestaurant.Application.ComboMaterials.DTOs;
using ERestaurant.Domain.Entity;

namespace ERestaurant.Application.ComboMaterials.AutoMapperProfiles
{
    public class ComboMaterialProfile : Profile
    {
        public ComboMaterialProfile()
        {
            CreateMap<ComboMaterial, ComboMaterialDTO>();
        }
    }

}
