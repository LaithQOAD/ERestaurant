using ERestaurant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERestaurant.Application.Orders.OrderServices
{
    public interface IOrderPricingService
    {
        Task RecalculateAsync(Order order);

    }
}
