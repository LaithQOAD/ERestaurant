using ERestaurant.Application.Services.MiddlewareInterfaces;

namespace ERestaurant.Testing.SharedHelperClass
{
    internal sealed class MockRequestTenant : IRequestTenant
    {
        public int TenantId { get; set; }
    }
}
