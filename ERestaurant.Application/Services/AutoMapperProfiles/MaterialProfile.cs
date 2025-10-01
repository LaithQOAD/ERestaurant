using AutoMapper;
using ERestaurant.Application.DTOs.MaterialDTOs;
using ERestaurant.Domain.Entity;

namespace ERestaurant.Application.Services.AutoMapperProfiles
{
    public class MaterialProfile : Profile
    {
        public MaterialProfile()
        {
            CreateMap<Material, MaterialDTO>()
                .ForMember(d => d.Name, o => o.MapFrom((source, destination, destinationMember, resolutionContext) =>
                {
                    var isArabic = resolutionContext.Items.ContainsKey("isArabic")
                                   && resolutionContext.Items["isArabic"] is true;
                    return isArabic ? source.NameAr : source.NameEn;
                }));

            CreateMap<CreateMaterialDTO, Material>()
            .ForMember(d => d.Id, o => o.Ignore());

            CreateMap<UpdateMaterialDTO, Material>()
                .ForMember(d => d.Id, o => o.Ignore());
        }
    }

}
