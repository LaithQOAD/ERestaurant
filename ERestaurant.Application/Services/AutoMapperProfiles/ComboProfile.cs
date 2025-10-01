using AutoMapper;
using ERestaurant.Application.DTOs.ComboDTOs;
using ERestaurant.Domain.Entity;

namespace ERestaurant.Application.Services.AutoMapperProfiles
{
    public class ComboProfile : Profile
    {
        public ComboProfile()
        {
            CreateMap<Combo, ComboDTO>()
                .ForMember(d => d.Name, o => o.MapFrom((source, destination, destinationMember, resolutionContext) =>
                {
                    var isArabic = resolutionContext.Items.ContainsKey("isArabic")
                                   && resolutionContext.Items["isArabic"] is true;
                    return isArabic ? source.NameAr : source.NameEn;
                }));

            CreateMap<CreateComboDTO, Combo>()
                .ForMember(d => d.ComboMaterial, opt => opt.MapFrom(src =>
                    src.Material.Select(m => new ComboMaterial
                    {
                        MaterialId = m.MaterialId,
                        Quantity = m.Quantity
                    })));

            CreateMap<UpdateComboDTO, Combo>()
                .ForMember(d => d.ComboMaterial, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.ComboMaterial ??= new List<ComboMaterial>();

                    var toRemove = dest.ComboMaterial
                        .Where(cm => !src.Material.Any(m => m.MaterialId == cm.MaterialId))
                        .ToList();

                    foreach (var r in toRemove)
                        dest.ComboMaterial.Remove(r);

                    foreach (var cm in dest.ComboMaterial)
                    {
                        var dto = src.Material.FirstOrDefault(m => m.MaterialId == cm.MaterialId);
                        if (dto is not null)
                        {
                            cm.Quantity = dto.Quantity;
                        }
                    }

                    var existingIds = dest.ComboMaterial.Select(x => x.MaterialId).ToHashSet();

                    var toAdd = src.Material.Where(m => !existingIds.Contains(m.MaterialId));
                    foreach (var a in toAdd)
                    {
                        dest.ComboMaterial.Add(new ComboMaterial
                        {
                            MaterialId = a.MaterialId,
                            Quantity = a.Quantity
                        });
                    }
                });
        }
    }

}
