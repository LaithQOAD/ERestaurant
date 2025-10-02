using ERestaurant.Application.Services.MiddlewareInterfaces;

namespace ERestaurant.Testing.SharedHelperClass
{
    internal sealed class MockRequestLanguage : IRequestLanguage
    {
        public string TwoLetter => "en";
        public bool IsArabic => false;
    }
}
