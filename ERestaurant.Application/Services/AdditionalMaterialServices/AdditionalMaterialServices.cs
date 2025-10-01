using AutoMapper;
using ERestaurant.Application.DTOs.AdditionalMaterialDTOs;
using ERestaurant.Application.Services.Middleware.Interfaces;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.Enums;
using ERestaurant.Domain.IUnitOfWork;
using ERestaurant.Infrastructure.HelperClass.Pagination;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.Services.AdditionalMaterialServices
{
    public sealed class AdditionalMaterialServices : IAdditionalMaterialServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRequestLanguage _language;

        public AdditionalMaterialServices(IUnitOfWork unitOfWork, IMapper mapper, IRequestLanguage language)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _language = language;
        }

        public async Task<List<AdditionalMaterialDTO>> FindAllAsync(
            string? searchQuery = null,
            MaterialUnit? unit = null,
            bool? isActive = null,
            decimal? priceFrom = null,
            decimal? priceTo = null,
            string? orderBy = "Name",
            string? orderByDirection = "ASC",
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            var mainQuery = _unitOfWork.AdditionalMaterial.Query();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                mainQuery = mainQuery.Where(x =>
                    (x.NameEn != null && EF.Functions.Like(x.NameEn, $"%{search}%")) ||
                    (x.NameAr != null && EF.Functions.Like(x.NameAr, $"%{search}%")));
            }

            if (unit.HasValue)
                mainQuery = mainQuery.Where(x => x.Unit == unit.Value);

            if (isActive.HasValue)
                mainQuery = mainQuery.Where(x => x.IsActive == isActive.Value);

            if (priceFrom.HasValue)
                mainQuery = mainQuery.Where(x => x.PricePerUnit >= priceFrom.Value);

            if (priceTo.HasValue)
                mainQuery = mainQuery.Where(x => x.PricePerUnit <= priceTo.Value);

            mainQuery = ApplyOrdering(mainQuery, orderBy, orderByDirection);

            var totalCount = await mainQuery.CountAsync();
            var paginationResult = PagingHelper.Compute(pageNumber, pageSize, totalCount);

            var resultList = await mainQuery
                .Skip(paginationResult.Skip)
                .Take(paginationResult.Take)
                .ToListAsync();

            return _mapper.Map<List<AdditionalMaterialDTO>>(resultList, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<AdditionalMaterialDTO?> FindByIdAsync(Guid id)
        {
            var mainQuery = _unitOfWork.AdditionalMaterial.Query();

            var materialEntity = await mainQuery.FirstOrDefaultAsync(x => x.Id == id);
            if (materialEntity is null) return null;

            return _mapper.Map<AdditionalMaterialDTO>(materialEntity, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        private static IQueryable<AdditionalMaterial> ApplyOrdering(
            IQueryable<AdditionalMaterial> query,
            string? orderBy,
            string? orderDirection)
        {
            var desc = string.Equals(orderDirection, "DESC", StringComparison.OrdinalIgnoreCase);

            return (orderBy?.ToLowerInvariant()) switch
            {
                "nameen" => desc ? query.OrderByDescending(m => m.NameEn) : query.OrderBy(m => m.NameEn),
                "namear" => desc ? query.OrderByDescending(m => m.NameAr) : query.OrderBy(m => m.NameAr),
                "unit" => desc ? query.OrderByDescending(m => m.Unit) : query.OrderBy(m => m.Unit),
                "isactive" => desc ? query.OrderByDescending(m => m.IsActive) : query.OrderBy(m => m.IsActive),
                "priceperunit" => desc ? query.OrderByDescending(m => m.PricePerUnit) : query.OrderBy(m => m.PricePerUnit),
                "tax" => desc ? query.OrderByDescending(m => m.Tax) : query.OrderBy(m => m.Tax),
                "createddate" => desc ? query.OrderByDescending(m => m.CreatedDate) : query.OrderBy(m => m.CreatedDate),
                _ => desc ? query.OrderByDescending(m => m.CreatedDate) : query.OrderBy(m => m.CreatedDate),
            };
        }

        public async Task<AdditionalMaterialDTO> AddAsync(CreateAdditionalMaterialDTO newAdditionalMaterial)
        {
            if (string.IsNullOrWhiteSpace(newAdditionalMaterial.NameEn))
                throw new ValidationException("NameEn is required.");
            if (string.IsNullOrWhiteSpace(newAdditionalMaterial.NameAr))
                throw new ValidationException("NameAr is required.");

            var materialEntity = new AdditionalMaterial
            {
                NameEn = newAdditionalMaterial.NameEn.Trim(),
                NameAr = newAdditionalMaterial.NameAr.Trim(),
                Unit = newAdditionalMaterial.Unit,
                PricePerUnit = decimal.Round(newAdditionalMaterial.PricePerUnit, 3, MidpointRounding.AwayFromZero),
                Tax = decimal.Round(newAdditionalMaterial.Tax, 3, MidpointRounding.AwayFromZero),
                ImageUrl = newAdditionalMaterial.ImageUrl,
                IsActive = newAdditionalMaterial.IsActive
            };

            await _unitOfWork.AdditionalMaterial.AddAsync(materialEntity);
            await _unitOfWork.SaveChangesAsync();

            var savedEntity = await _unitOfWork.AdditionalMaterial.FindByIdAsync(materialEntity.Id);
            return _mapper.Map<AdditionalMaterialDTO>(savedEntity, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<AdditionalMaterialDTO> UpdateAsync(UpdateAdditionalMaterialDTO updatedAdditionalMaterial)
        {
            var materialEntity = await _unitOfWork.AdditionalMaterial.FindByIdAsync(updatedAdditionalMaterial.Id)
                ?? throw new KeyNotFoundException("Additional material not found");

            if (string.IsNullOrWhiteSpace(updatedAdditionalMaterial.NameEn))
                throw new ValidationException("NameEn is required.");
            if (string.IsNullOrWhiteSpace(updatedAdditionalMaterial.NameAr))
                throw new ValidationException("NameAr is required.");

            materialEntity.NameEn = updatedAdditionalMaterial.NameEn.Trim();
            materialEntity.NameAr = updatedAdditionalMaterial.NameAr.Trim();
            materialEntity.Unit = updatedAdditionalMaterial.Unit;
            materialEntity.PricePerUnit = decimal.Round(updatedAdditionalMaterial.PricePerUnit, 3, MidpointRounding.AwayFromZero);
            materialEntity.Tax = decimal.Round(updatedAdditionalMaterial.Tax, 3, MidpointRounding.AwayFromZero);
            materialEntity.ImageUrl = updatedAdditionalMaterial.ImageUrl;
            materialEntity.IsActive = updatedAdditionalMaterial.IsActive;

            await _unitOfWork.AdditionalMaterial.UpdateAsync(materialEntity);
            await _unitOfWork.SaveChangesAsync();

            var savedEntity = await _unitOfWork.AdditionalMaterial.FindByIdAsync(materialEntity.Id);
            return _mapper.Map<AdditionalMaterialDTO>(savedEntity, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.AdditionalMaterial.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
