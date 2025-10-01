using AutoMapper;
using ERestaurant.Application.DTOs.ComboDTOs;
using ERestaurant.Application.Services.Middleware.Interfaces;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.IUnitOfWork;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.Services.ComboMaterialServices
{
    public sealed class ComboMaterialServices : IComboMaterialServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRequestLanguage _language;

        public ComboMaterialServices(IUnitOfWork unitOfWork, IMapper mapper, IRequestLanguage language)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _language = language;
        }

        public async Task<ComboDTO> AddMaterialAsync(Guid comboId, Guid materialId, int quantity)
        {
            EnsurePositive(quantity);

            var comboEntity = await GetComboWithMaterialAsync(comboId);
            _ = await _unitOfWork.Material.FindByIdAsync(materialId)
                ?? throw new KeyNotFoundException("Material not found");

            var line = comboEntity.ComboMaterial.FirstOrDefault(cm => cm.MaterialId == materialId);
            if (line is null)
            {
                comboEntity.ComboMaterial.Add(new ComboMaterial
                {
                    ComboId = comboEntity.Id,
                    MaterialId = materialId,
                    Quantity = quantity
                });
            }
            else
            {
                checked { line.Quantity += quantity; }
            }

            if (!comboEntity.IsActive)
                comboEntity.IsActive = true;

            return await SaveRecalculateAndReturnAsync(comboEntity);
        }

        public async Task<ComboDTO> UpdateQuantityAsync(Guid comboId, Guid materialId, int newQuantity)
        {
            EnsurePositive(newQuantity);

            var comboEntity = await GetComboWithMaterialAsync(comboId);
            var line = comboEntity.ComboMaterial.FirstOrDefault(cm => cm.MaterialId == materialId)
                ?? throw new KeyNotFoundException("This material is not part of the combo.");

            line.Quantity = newQuantity;

            if (!comboEntity.IsActive)
                comboEntity.IsActive = true;

            return await SaveRecalculateAndReturnAsync(comboEntity);
        }

        public async Task<ComboDTO> RemoveMaterialAsync(Guid comboId, Guid materialId)
        {
            var comboEntity = await GetComboWithMaterialAsync(comboId);
            var line = comboEntity.ComboMaterial.FirstOrDefault(cm => cm.MaterialId == materialId)
                ?? throw new KeyNotFoundException("This material is not part of the combo.");

            comboEntity.ComboMaterial.Remove(line);

            if (comboEntity.ComboMaterial.Count == 0)
                comboEntity.IsActive = false;

            return await SaveRecalculateAndReturnAsync(comboEntity);
        }

        private static void EnsurePositive(int quantity)
        {
            if (quantity <= 0) throw new ValidationException("Quantity must be greater than 0.");
        }

        private async Task<Combo> GetComboWithMaterialAsync(Guid comboId)
        {
            var comboEntity = await _unitOfWork.Combo.FindByIdAsync(comboId);
            if (comboEntity is null)
                throw new KeyNotFoundException("Combo not found");

            comboEntity.ComboMaterial ??= new List<ComboMaterial>();
            return comboEntity;
        }

        private async Task<ComboDTO> SaveRecalculateAndReturnAsync(Combo combo)
        {
            await RecalculateAsync(combo);

            foreach (var cm in combo.ComboMaterial)
            {
                if (cm.Material != null && cm.MaterialId == Guid.Empty) cm.MaterialId = cm.Material.Id;
                cm.Material = null;
                cm.Combo = null;
            }

            await _unitOfWork.Combo.UpdateAsync(combo);
            await _unitOfWork.SaveChangesAsync();

            var withIncludes = await _unitOfWork.Combo.FindByIdAsync(combo.Id);
            return _mapper.Map<ComboDTO>(withIncludes, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        private static decimal Round3(decimal v) => decimal.Round(v, 3, MidpointRounding.AwayFromZero);

        private async Task RecalculateAsync(Combo comboEntity)
        {
            decimal totalBeforeTax = 0m;
            decimal totalTaxValue = 0m;

            foreach (var cm in comboEntity.ComboMaterial)
            {
                var material = cm.Material
                    ?? await _unitOfWork.Material.FindByIdAsync(cm.MaterialId)
                    ?? throw new KeyNotFoundException($"Material {cm.MaterialId} not found");

                var lineBefore = material.PricePerUnit * cm.Quantity;
                var lineTax = Round3(lineBefore * material.Tax);

                totalBeforeTax += Round3(lineBefore);
                totalTaxValue += lineTax;
            }

            if (totalBeforeTax <= 0m)
            {
                comboEntity.Price = 0m;
                comboEntity.Tax = 0m;
                return;
            }

            comboEntity.Price = Round3(totalBeforeTax);
            var effectiveTaxRate = totalTaxValue / totalBeforeTax;
            comboEntity.Tax = Round3(effectiveTaxRate);
        }
    }
}
