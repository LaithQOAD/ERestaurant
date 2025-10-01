using ERestaurant.Application.Services.Middleware.Interfaces;
using System.Globalization;

namespace ERestaurant.API.Middlewares.LanguageMiddleware
{
    public class RequestLanguage : IRequestLanguage
    {
        public string TwoLetter => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        public bool IsArabic => TwoLetter == "ar";
    }
}
