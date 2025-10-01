using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace ERestaurant.Application.Services.Localization
{
    public sealed class HeaderRequestCultureProvider : RequestCultureProvider
    {
        public const string HeaderName = "X-Accept-Language";

        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext context)
        {
            var raw = context.Request.Headers[HeaderName].ToString();

            static string Map(string? s) =>
                string.IsNullOrWhiteSpace(s) ? "en" :
                s.StartsWith("ar", StringComparison.OrdinalIgnoreCase) ? "ar" : "en";

            var lang = Map(raw);
            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(lang, lang));
        }
    }

    public static class LocalizationExtensions
    {
        public static IServiceCollection AddHeaderLocalization(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supported = new[] { new CultureInfo("en"), new CultureInfo("ar") };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supported;
                options.SupportedUICultures = supported;

                options.RequestCultureProviders.Insert(0, new HeaderRequestCultureProvider());
            });

            return services;
        }

        public static IApplicationBuilder UseHeaderLocalization(this IApplicationBuilder app)
        {
            var locOptions = app.ApplicationServices
                                 .GetRequiredService<IOptions<RequestLocalizationOptions>>()
                                 .Value;
            app.UseRequestLocalization(locOptions);
            return app;
        }
    }

}
