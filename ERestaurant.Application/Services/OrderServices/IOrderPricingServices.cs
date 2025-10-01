using ERestaurant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERestaurant.Application.Services.OrderServices
{
    public interface IOrderPricingServices
    {
        Task RecalculateAsync(Order order);

    }
}
