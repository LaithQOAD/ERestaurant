using AutoMapper;
using ERestaurant.Application.AdditionalMaterials.AdditionalMaterialDTOs;
using ERestaurant.Domain.Entity;

namespace ERestaurant.Application.AdditionalMaterials.AutoMapperProfiles
{
    public class AdditionalMaterialProfile : Profile
    {
        public AdditionalMaterialProfile()
        {
            CreateMap<AdditionalMaterial, AdditionalMaterialDTO>()
                .ForMember(d => d.Name, o => o.MapFrom((source, destination, destinationMember, resolutionContext) =>
                {
                    var isArabic = resolutionContext.Items.ContainsKey("isArabic")
                                    && resolutionContext.Items["isArabic"] is true;
                    return isArabic ? source.NameAr : source.NameEn;
                }));

            CreateMap<CreateAdditionalMaterialDTO, AdditionalMaterial>();

            CreateMap<UpdateAdditionalMaterialDTO, AdditionalMaterial>();

        }
    }
}
