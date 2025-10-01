using Microsoft.OpenApi.Models;

namespace ERestaurant.API.Middlewares.SwaggerService
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerWithTenantAndLanguage(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ERestaurant API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Tenant", new OpenApiSecurityScheme
                {
                    Description = "Tenant Id header (e.g. 1, 2, ...)",
                    Name = "X-Tenant-Id",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityDefinition("Language", new OpenApiSecurityScheme
                {
                    Description = "Language header (e.g. ar:Arabic , en:English)",
                    Name = "X-Accept-Language",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Tenant" }
                        },
                        Array.Empty<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Language" }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
