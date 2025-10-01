using AutoMapper;
using ERestaurant.Application.DTOs.OrderDTOs;
using ERestaurant.Application.DTOs.OrderItemDTOs; // IOrderItemInputDTO, Create/Update DTOs
using ERestaurant.Application.Services.Middleware.Interfaces;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.IUnitOfWork;
using ERestaurant.Infrastructure.HelperClass.Pagination;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.Services.OrderServices
{
    public sealed class OrderServices : IOrderServices, IOrderPricingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRequestLanguage _language;

        public OrderServices(IUnitOfWork unitOfWork, IMapper mapper, IRequestLanguage language)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _language = language;
        }

        public async Task<List<OrderDTO>> FindAllAsync(
            string? searchQuery = null,
            DateTimeOffset? dateFrom = null,
            DateTimeOffset? dateTo = null,
            bool? isActive = null,
            string? orderBy = "OrderDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            var query = _unitOfWork.Order.Query();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var customerNameSearch = searchQuery.Trim();
                query = query.Where(o =>
                    o.CustomerName != null &&
                    EF.Functions.Like(o.CustomerName, $"%{customerNameSearch}%"));
            }

            if (dateFrom.HasValue)
            {
                var startDate = new DateTimeOffset(dateFrom.Value.Date, TimeSpan.Zero);
                query = query.Where(o => o.OrderDate >= startDate);
            }

            if (dateTo.HasValue)
            {
                var endDateExclusive = new DateTimeOffset(dateTo.Value.Date.AddDays(1), TimeSpan.Zero);
                query = query.Where(o => o.OrderDate < endDateExclusive);
            }

            if (isActive.HasValue)
                query = query.Where(o => o.IsActive == isActive.Value);

            query = ApplyOrdering(query, orderBy, orderByDirection);

            var totalCount = await query.CountAsync();
            var pagination = PagingHelper.Compute(pageNumber, pageSize, totalCount);

            var pagedOrders = await query
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();

            if (pagedOrders.Count == 0)
                throw new KeyNotFoundException("No any combo in the specfic filter");

            return _mapper.Map<List<OrderDTO>>(pagedOrders, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<OrderDTO?> FindByIdAsync(Guid id)
        {
            var orderQuery = await _unitOfWork.Order.FindByIdAsync(id);

            if (orderQuery == null)
                return null;

            return _mapper.Map<OrderDTO>(orderQuery, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<OrderDTO> CreateAsync(CreateOrderDTO newOrder)
        {
            ValidateItems(newOrder.OrderItem);

            var orderEntity = new Order
            {
                CustomerName = newOrder.CustomerName,
                CustomerPhone = newOrder.CustomerPhone,
                IsActive = true,
                OrderDate = DateTimeOffset.UtcNow,
                OrderItem = new List<OrderItem>()
            };

            RebuildItems(orderEntity, newOrder.OrderItem);
            await RecalculateAsync(orderEntity);

            await _unitOfWork.Order.AddAsync(orderEntity);
            await _unitOfWork.SaveChangesAsync();

            var orderWithIncludes = await _unitOfWork.Order.FindByIdAsync(orderEntity.Id);

            return _mapper.Map<OrderDTO>(orderWithIncludes, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task<OrderDTO> UpdateAsync(UpdateOrderDTO updatedOrder)
        {
            var orderEntity = await _unitOfWork.Order.FindByIdAsync(updatedOrder.Id);
            if (orderEntity is null)
                throw new KeyNotFoundException("Order not found");

            orderEntity.CustomerName = updatedOrder.CustomerName;
            orderEntity.CustomerPhone = updatedOrder.CustomerPhone;
            orderEntity.OrderDate = DateTimeOffset.UtcNow;

            if (updatedOrder.OrderItem is not null && updatedOrder.OrderItem.Count > 0)
            {
                ValidateItems(updatedOrder.OrderItem);
                orderEntity.OrderItem.Clear();
                RebuildItems(orderEntity, updatedOrder.OrderItem);
            }

            await RecalculateAsync(orderEntity);

            await _unitOfWork.Order.UpdateAsync(orderEntity);
            await _unitOfWork.SaveChangesAsync();

            var orderWithIncludes = await _unitOfWork.Order.FindByIdAsync(orderEntity.Id);

            return _mapper.Map<OrderDTO>(orderWithIncludes, opt => opt.Items["isArabic"] = _language.IsArabic);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Order.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        private static IQueryable<Order> ApplyOrdering(IQueryable<Order> query, string? orderByField, string? direction)
        {
            var desc = string.Equals(direction, "DESC", StringComparison.OrdinalIgnoreCase);
            return (orderByField?.ToLowerInvariant()) switch
            {
                "orderdate" => desc ? query.OrderByDescending(o => o.OrderDate) : query.OrderBy(o => o.OrderDate),
                "customername" => desc ? query.OrderByDescending(o => o.CustomerName) : query.OrderBy(o => o.CustomerName),
                "customerphone" => desc ? query.OrderByDescending(o => o.CustomerPhone) : query.OrderBy(o => o.CustomerPhone),
                "totalpricebeforetax" => desc ? query.OrderByDescending(o => o.TotalPriceBeforeTax) : query.OrderBy(o => o.TotalPriceBeforeTax),
                "totaltax" => desc ? query.OrderByDescending(o => o.TotalTax) : query.OrderBy(o => o.TotalTax),
                "totalpriceaftertax" => desc ? query.OrderByDescending(o => o.TotalPriceAfterTax) : query.OrderBy(o => o.TotalPriceAfterTax),
                "isactive" => desc ? query.OrderByDescending(o => o.IsActive) : query.OrderBy(o => o.IsActive),
                _ => desc ? query.OrderByDescending(o => o.OrderDate) : query.OrderBy(o => o.OrderDate),
            };
        }
        private readonly record struct ItemRef(Guid? MaterialId, Guid? ComboId, Guid? AdditionalMaterialId, int Quantity);
        private static ItemRef ToItemRef(IOrderItemInputDTO it)
            => new(it.MaterialId, it.ComboId, it.AdditionalMaterialId, it.Quantity);
        private static void ValidateItems<T>(IEnumerable<T> items)
         where T : IOrderItemInputDTO
        {
            if (items is null || !items.Any())
                throw new ValidationException("Order must contain at least one item.");

            foreach (var it in items.Select(x => ToItemRef(x)))
            {
                int refs = (it.MaterialId.HasValue ? 1 : 0)
                         + (it.ComboId.HasValue ? 1 : 0)
                         + (it.AdditionalMaterialId.HasValue ? 1 : 0);

                if (refs != 1)
                    throw new ValidationException("Each item must reference exactly one of Material or Combo or AdditionalMaterial.");

                if (it.Quantity <= 0)
                    throw new ValidationException("Item quantity must be greater than 0.");
            }
        }
        private static void RebuildItems<T>(Order order, IEnumerable<T> items)
         where T : IOrderItemInputDTO
        {
            order.OrderItem ??= new List<OrderItem>();

            foreach (var r in items.Select(x => ToItemRef(x)))
            {
                order.OrderItem.Add(new OrderItem
                {
                    MaterialId = r.MaterialId,
                    ComboId = r.ComboId,
                    AdditionalMaterialId = r.AdditionalMaterialId,
                    Quantity = r.Quantity
                });
            }
        }

        public async Task RecalculateAsync(Order orderEntity)
        {
            decimal totalBeforeTax = 0m, totalTax = 0m, totalAfterTax = 0m;

            foreach (var orderItem in orderEntity.OrderItem)
            {
                decimal unitPrice, taxRate;

                if (orderItem.MaterialId.HasValue)
                {
                    var materialEntity = await _unitOfWork.Material.FindByIdAsync(orderItem.MaterialId.Value)
                        ?? throw new KeyNotFoundException("Material not found");
                    unitPrice = materialEntity.PricePerUnit;
                    taxRate = materialEntity.Tax; // 0..1
                }
                else if (orderItem.ComboId.HasValue)
                {
                    var comboEntity = await _unitOfWork.Combo.FindByIdAsync(orderItem.ComboId.Value)
                        ?? throw new KeyNotFoundException("Combo not found");
                    unitPrice = comboEntity.Price;
                    taxRate = comboEntity.Tax;
                }
                else
                {
                    var additionalMaterialEntity = await _unitOfWork.AdditionalMaterial.FindByIdAsync(orderItem.AdditionalMaterialId!.Value)
                        ?? throw new KeyNotFoundException("Additional material not found");
                    unitPrice = additionalMaterialEntity.PricePerUnit;
                    taxRate = additionalMaterialEntity.Tax;
                }

                var lineBefore = unitPrice * orderItem.Quantity;
                var lineTax = decimal.Round(lineBefore * taxRate, 3, MidpointRounding.AwayFromZero);
                var lineAfter = lineBefore + lineTax;

                orderItem.PriceBeforeTax = decimal.Round(lineBefore, 3, MidpointRounding.AwayFromZero);
                orderItem.Tax = lineTax;
                orderItem.PriceAfterTax = decimal.Round(lineAfter, 3, MidpointRounding.AwayFromZero);

                totalBeforeTax += orderItem.PriceBeforeTax;
                totalTax += orderItem.Tax;
                totalAfterTax += orderItem.PriceAfterTax;
            }

            orderEntity.TotalPriceBeforeTax = decimal.Round(totalBeforeTax, 3, MidpointRounding.AwayFromZero);
            orderEntity.TotalTax = decimal.Round(totalTax, 3, MidpointRounding.AwayFromZero);
            orderEntity.TotalPriceAfterTax = decimal.Round(totalAfterTax, 3, MidpointRounding.AwayFromZero);
        }
    }
}
