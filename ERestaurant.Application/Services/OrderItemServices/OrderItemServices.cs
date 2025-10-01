using AutoMapper;
using ERestaurant.Application.DTOs.OrderDTOs;
using ERestaurant.Application.Services.Middleware.Interfaces;
using ERestaurant.Application.Services.OrderServices;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.IUnitOfWork;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.Services.OrderItemServices
{
    public sealed class OrderItemServices : IOrderItemServices
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IRequestLanguage _language;
        private readonly IOrderPricingServices _orderPricingServices;

        public OrderItemServices(IUnitOfWork uow,
            IMapper mapper,
            IRequestLanguage language,
            IOrderPricingServices orderPricingServices)
        {
            _uow = uow;
            _mapper = mapper;
            _language = language;
            _orderPricingServices = orderPricingServices;
        }

        public async Task<OrderDTO> AddMaterialAsync(Guid orderId, Guid materialId, int quantity)
        {
            EnsurePositive(quantity);


            var order = await _uow.Order.FindByIdAsync(orderId) ?? throw new KeyNotFoundException("Order not found");
            _ = await _uow.Material.FindByIdAsync(materialId) ?? throw new KeyNotFoundException("Material not found");


            var line = order.OrderItem.FirstOrDefault(i => i.MaterialId == materialId && i.ComboId == null && i.AdditionalMaterialId == null);
            if (line is null)
            {
                order.OrderItem.Add(new OrderItem
                {
                    MaterialId = materialId,
                    Quantity = quantity
                });
            }
            else
            {
                checked { line.Quantity += quantity; }
            }


            if (!order.IsActive) order.IsActive = true;


            return await SaveRecalculateAndReturnAsync(order);
        }

        public async Task<OrderDTO> AddComboAsync(Guid orderId, Guid comboId, int quantity)
        {
            EnsurePositive(quantity);


            var order = await _uow.Order.FindByIdAsync(orderId) ?? throw new KeyNotFoundException("Order not found");
            _ = await _uow.Combo.FindByIdAsync(comboId) ?? throw new KeyNotFoundException("Combo not found");


            var line = order.OrderItem.FirstOrDefault(i => i.ComboId == comboId && i.MaterialId == null && i.AdditionalMaterialId == null);
            if (line is null)
            {
                order.OrderItem.Add(new OrderItem
                {
                    ComboId = comboId,
                    Quantity = quantity
                });
            }
            else
            {
                checked { line.Quantity += quantity; }
            }


            if (!order.IsActive) order.IsActive = true;


            return await SaveRecalculateAndReturnAsync(order);
        }

        public async Task<OrderDTO> AddAdditionalMaterialAsync(Guid orderId, Guid additionalMaterialId, int quantity)
        {
            EnsurePositive(quantity);


            var order = await _uow.Order.FindByIdAsync(orderId) ?? throw new KeyNotFoundException("Order not found");
            _ = await _uow.AdditionalMaterial.FindByIdAsync(additionalMaterialId) ?? throw new KeyNotFoundException("Additional material not found");


            var line = order.OrderItem.FirstOrDefault(i => i.AdditionalMaterialId == additionalMaterialId && i.MaterialId == null && i.ComboId == null);
            if (line is null)
            {
                order.OrderItem.Add(new OrderItem
                {
                    AdditionalMaterialId = additionalMaterialId,
                    Quantity = quantity
                });
            }
            else
            {
                checked { line.Quantity += quantity; }
            }


            if (!order.IsActive) order.IsActive = true;


            return await SaveRecalculateAndReturnAsync(order);
        }

        public async Task<OrderDTO> RemoveAsync(Guid orderId, Guid referenceId)
        {
            var order = await _uow.Order.FindByIdAsync(orderId)
                ?? throw new KeyNotFoundException("Order not found");

            var matches = order.OrderItem
                .Where(i => i.MaterialId == referenceId
                         || i.ComboId == referenceId
                         || i.AdditionalMaterialId == referenceId)
                .ToList();

            if (matches.Count == 0)
                throw new KeyNotFoundException("Item reference not found in this order.");

            if (matches.Count > 1)
                throw new ValidationException("Ambiguous reference id: matches multiple item types.");

            order.OrderItem.Remove(matches[0]);

            if (order.OrderItem.Count == 0)
                order.IsActive = false;

            return await SaveRecalculateAndReturnAsync(order);
        }

        private static void EnsurePositive(int q)
        {
            if (q <= 0) throw new ValidationException("Quantity must be greater than 0.");
        }
        private async Task<OrderDTO> SaveRecalculateAndReturnAsync(Order order)
        {
            await _orderPricingServices.RecalculateAsync(order);

            foreach (var oi in order.OrderItem)
            {
                if (oi.Material != null && oi.MaterialId == null) oi.MaterialId = oi.Material.Id;
                if (oi.Combo != null && oi.ComboId == null) oi.ComboId = oi.Combo.Id;
                if (oi.AdditionalMaterial != null && oi.AdditionalMaterialId == null) oi.AdditionalMaterialId = oi.AdditionalMaterial.Id;

                oi.Material = null;
                oi.Combo = null;
                oi.AdditionalMaterial = null;
                oi.Order = null;
            }

            await _uow.Order.UpdateAsync(order);
            await _uow.SaveChangesAsync();

            var withIncludes = await _uow.Order.FindByIdAsync(order.Id);
            return _mapper.Map<OrderDTO>(withIncludes, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

    }
}


