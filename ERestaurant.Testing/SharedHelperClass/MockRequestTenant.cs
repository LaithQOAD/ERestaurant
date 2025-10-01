using ERestaurant.Application.Services.Middleware.Interfaces;

namespace ERestaurant.Testing.SharedHelperClass
{
    internal sealed class MockRequestTenant : IRequestTenant
    {
        public int TenantId { get; set; }
    }
}
