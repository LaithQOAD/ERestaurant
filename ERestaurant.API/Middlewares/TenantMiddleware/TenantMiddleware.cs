using ERestaurant.Application.Services.Middleware.Interfaces;

namespace ERestaurant.API.Middlewares.TenantMiddleware
{

    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var requestContext = context.RequestServices
                .GetRequiredService<IRequestTenant>() as RequestTenant;

            var tenantHeader = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
            if (!int.TryParse(tenantHeader, out var tenantId) || tenantId <= 0)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("X-Tenant-Id header is missing or invalid.");
                return;
            }

            requestContext!.TenantId = tenantId;
            await _next(context);
        }

    }
}
