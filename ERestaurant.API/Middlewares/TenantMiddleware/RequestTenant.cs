using ERestaurant.Application.Services.MiddlewareInterfaces;
namespace ERestaurant.API.Middlewares.TenantMiddleware
{
    public class RequestTenant : IRequestTenant
    {
        public int TenantId { get; set; }
    }
}
