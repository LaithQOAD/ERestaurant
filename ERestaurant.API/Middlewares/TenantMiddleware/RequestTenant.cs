using ERestaurant.Application.Services.Middleware.Interfaces;
namespace ERestaurant.API.Middlewares.TenantMiddleware
{
    public class RequestTenant : IRequestTenant
    {
        public int TenantId { get; set; }
    }
}
