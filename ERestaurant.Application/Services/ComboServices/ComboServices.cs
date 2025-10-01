using AutoMapper;
using ERestaurant.Application.DTOs.AdditionalMaterialDTOs;
using ERestaurant.Application.DTOs.ComboDTOs;
using ERestaurant.Application.Services.Middleware.Interfaces;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.IUnitOfWork;
using ERestaurant.Infrastructure.HelperClass.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Application.Services.ComboServices
{
    public sealed class ComboServices : IComboServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRequestLanguage _language;

        public ComboServices(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IRequestLanguage language
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _language = language;
        }

        public async Task<ComboWithAdditionalMaterialDTO> FindAllWithAdditionsAsync(
            string? searchComboNameQuery = null,
            string? filterComboNameQuery = null,
            bool? isActive = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? orderBy = "CreatedDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            var mainQuery = _unitOfWork.Combo.Query();

            if (!string.IsNullOrWhiteSpace(searchComboNameQuery))
            {
                var searchQuery = searchComboNameQuery.Trim();
                mainQuery = mainQuery.Where(c => EF.Functions.Like(c.NameEn, $"%{searchQuery}%") ||
                                 EF.Functions.Like(c.NameAr, $"%{searchQuery}%"));
            }
            if (!string.IsNullOrWhiteSpace(filterComboNameQuery))
            {
                var filterQuery = filterComboNameQuery.Trim();
                mainQuery = mainQuery.Where(c => c.NameEn == filterQuery || c.NameAr == filterQuery);
            }
            if (isActive.HasValue) mainQuery = mainQuery.Where(c => c.IsActive == isActive.Value);

            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
                (minPrice, maxPrice) = (maxPrice, minPrice);

            if (minPrice.HasValue) mainQuery = mainQuery.Where(x => x.Price >= minPrice.Value);
            if (maxPrice.HasValue) mainQuery = mainQuery.Where(x => x.Price <= maxPrice.Value);

            mainQuery = ApplyOrdering(mainQuery, orderBy, orderByDirection);

            var total = await mainQuery.CountAsync();
            var paginationResult = PagingHelper.Compute(pageNumber, pageSize, total);

            var combos = await mainQuery.Skip(paginationResult.Skip).Take(paginationResult.Take).ToListAsync();

            var comboDtos = _mapper.Map<List<ComboDTO>>(
                combos, opt => opt.Items["isArabic"] = _language.IsArabic);

            var additionalMaterialQuery = _unitOfWork.AdditionalMaterial.Query()
                .Where(a => a.IsActive);

            additionalMaterialQuery = _language.IsArabic ? additionalMaterialQuery.OrderBy(a => a.NameAr) : additionalMaterialQuery.OrderBy(a => a.NameEn);

            var additions = await additionalMaterialQuery.ToListAsync();

            var additionDtos = _mapper.Map<List<AdditionalMaterialDTO>>(
                additions, opt => opt.Items["isArabic"] = _language.IsArabic);

            return new ComboWithAdditionalMaterialDTO
            {
                Combo = comboDtos,
                AdditionalMaterial = additionDtos
            };
        }

        public async Task<List<ComboDTO>> FindAllAsync(
            string? searchComboNameQuery = null,
            string? filterComboNameQuery = null,
            bool? isActive = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? orderBy = "CreatedDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            var mainQuery = _unitOfWork.Combo.Query();

            if (!string.IsNullOrWhiteSpace(searchComboNameQuery))
            {
                var searchQuery = searchComboNameQuery.Trim();
                mainQuery = mainQuery.Where(c => EF.Functions.Like(c.NameEn, $"%{searchQuery}%") ||
                                 EF.Functions.Like(c.NameAr, $"%{searchQuery}%"));
            }
            if (!string.IsNullOrWhiteSpace(filterComboNameQuery))
            {
                var filterQuery = filterComboNameQuery.Trim();
                mainQuery = mainQuery.Where(c => c.NameEn == filterQuery || c.NameAr == filterQuery);
            }
            if (isActive.HasValue) mainQuery = mainQuery.Where(c => c.IsActive == isActive.Value);

            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
                (minPrice, maxPrice) = (maxPrice, minPrice);

            if (minPrice.HasValue) mainQuery = mainQuery.Where(x => x.Price >= minPrice.Value);
            if (maxPrice.HasValue) mainQuery = mainQuery.Where(x => x.Price <= maxPrice.Value);

            mainQuery = ApplyOrdering(mainQuery, orderBy, orderByDirection);

            var total = await mainQuery.CountAsync();
            var paginationResult = PagingHelper.Compute(pageNumber, pageSize, total);

            var combos = await mainQuery.Skip(paginationResult.Skip).Take(paginationResult.Take).ToListAsync();

            return _mapper.Map<List<ComboDTO>>(
                combos, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<ComboDTO?> FindByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Combo.Query()
                .Include(c => c.ComboMaterial).ThenInclude(cm => cm.Material)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity is null) throw new KeyNotFoundException("Combo not found");

            return _mapper.Map<ComboDTO>(entity,
                opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<ComboDTO> CreateAsync(CreateComboDTO createdComboRequest)
        {
            var entity = _mapper.Map<Combo>(createdComboRequest);
            await _unitOfWork.Combo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ComboDTO>(entity,
                opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<ComboDTO> UpdateAsync(UpdateComboDTO updatedComboRequest)
        {
            var existing = await _unitOfWork.Combo.FindByIdAsync(updatedComboRequest.Id);
            if (existing is null) throw new KeyNotFoundException("Combo not found");

            _mapper.Map(updatedComboRequest, existing);
            await _unitOfWork.Combo.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ComboDTO>(existing,
                opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _unitOfWork.Combo.FindByIdAsync(id);
            if (existing is null) throw new KeyNotFoundException("Combo not found");

            await _unitOfWork.Combo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        private static IQueryable<Combo> ApplyOrdering(IQueryable<Combo> q, string? by, string? dir)
        {
            var desc = string.Equals(dir, "DESC", StringComparison.OrdinalIgnoreCase);
            return (by?.ToLowerInvariant()) switch
            {
                "price" => desc ? q.OrderByDescending(c => c.Price) : q.OrderBy(c => c.Price),
                "tax" => desc ? q.OrderByDescending(c => c.Tax) : q.OrderBy(c => c.Tax),
                "nameen" => desc ? q.OrderByDescending(c => c.NameEn) : q.OrderBy(c => c.NameEn),
                "namear" => desc ? q.OrderByDescending(c => c.NameAr) : q.OrderBy(c => c.NameAr),
                "isactive" => desc ? q.OrderByDescending(c => c.IsActive) : q.OrderBy(c => c.IsActive),
                _ => desc ? q.OrderByDescending(c => c.CreatedDate) : q.OrderBy(c => c.CreatedDate),
            };
        }
    }
}
