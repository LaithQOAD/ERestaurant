using AutoMapper;
using ERestaurant.Application.Materials.DTOs;
using ERestaurant.Application.Services.MiddlewareInterfaces;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.Enums;
using ERestaurant.Domain.IUnitOfWork;
using ERestaurant.Infrastructure.HelperClass.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Application.Materials.MaterialServices
{
    public sealed class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRequestLanguage _language;

        public MaterialService(IUnitOfWork unitOfWork, IMapper mapper, IRequestLanguage language)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _language = language;
        }
        public async Task<List<MaterialDTO>> FindAllAsync(FindAllMaterialParameterDTO param)
        {
            IQueryable<Material> mainQuery = _unitOfWork.Material.Query();

            if (!string.IsNullOrWhiteSpace(param.searchNameQuery))
            {
                var s = param.searchNameQuery.Trim();
                mainQuery = mainQuery.Where(m =>
                    EF.Functions.Like(m.NameEn, $"%{s}%") ||
                    EF.Functions.Like(m.NameAr, $"%{s}%"));
            }

            if (param.isActive.HasValue)
                mainQuery = mainQuery.Where(m => m.IsActive == param.isActive.Value);

            if (param.unit.HasValue)
                mainQuery = mainQuery.Where(m => m.Unit == param.unit.Value);

            mainQuery = ApplyOrdering(mainQuery, param.orderBy, param.orderByDirection);

            var pageNumber = param.pageNumber ?? 1;
            var pageSize = param.pageSize ?? 10;

            var total = await mainQuery.CountAsync();
            var paginationResult = PagingHelper.Compute(pageNumber, pageSize, total);

            var entities = await mainQuery
                .Skip(paginationResult.Skip)
                .Take(paginationResult.Take)
                .ToListAsync();

            return _mapper.Map<List<MaterialDTO>>(
                entities, opt => opt.Items["isArabic"] = _language.IsArabic);
        }


        public async Task<MaterialDTO?> FindByIdAsync(Guid id)
        {
            var mainQuery = _unitOfWork.Material.Query();

            var entity = await mainQuery.FirstOrDefaultAsync(c => c.Id == id);

            return entity is null ? null : _mapper.Map<MaterialDTO>(entity, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<MaterialDTO> CreateAsync(CreateMaterialDTO newMaterial)
        {
            var entity = _mapper.Map<Material>(newMaterial);

            await _unitOfWork.Material.AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MaterialDTO>(entity, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<MaterialDTO> UpdateAsync(UpdateMaterialDTO updatedMaterial)
        {
            var existing = await _unitOfWork.Material.FindByIdAsync(updatedMaterial.Id);
            if (existing is null) throw new KeyNotFoundException("Material not found");

            _mapper.Map(updatedMaterial, existing);
            await _unitOfWork.Material.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MaterialDTO>(existing, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Material.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        private static IQueryable<Material> ApplyOrdering(IQueryable<Material> queryDB, string? orderBy, string? orderDirection)
        {
            var desc = string.Equals(orderDirection, "DESC", StringComparison.OrdinalIgnoreCase);

            return (orderBy?.ToLowerInvariant()) switch
            {
                "nameen" => desc ? queryDB.OrderByDescending(m => m.NameEn) : queryDB.OrderBy(m => m.NameEn),
                "namear" => desc ? queryDB.OrderByDescending(m => m.NameAr) : queryDB.OrderBy(m => m.NameAr),
                "unit" => desc ? queryDB.OrderByDescending(m => m.Unit) : queryDB.OrderBy(m => m.Unit),
                "isactive" => desc ? queryDB.OrderByDescending(m => m.IsActive) : queryDB.OrderBy(m => m.IsActive),
                "priceperunit" => desc ? queryDB.OrderByDescending(m => m.PricePerUnit) : queryDB.OrderBy(m => m.PricePerUnit),
                "tax" => desc ? queryDB.OrderByDescending(m => m.Tax) : queryDB.OrderBy(m => m.Tax),
                "createddate" => desc ? queryDB.OrderByDescending(m => m.CreatedDate) : queryDB.OrderBy(m => m.CreatedDate),
                _ => desc ? queryDB.OrderByDescending(m => m.CreatedDate) : queryDB.OrderBy(m => m.CreatedDate),
            };
        }
    }
}