using ERestaurant.API.Middlewares.GlobalExceptionMiddleware;
using ERestaurant.API.Middlewares.LanguageMiddleware;
using ERestaurant.API.Middlewares.SwaggerService;
using ERestaurant.API.Middlewares.TenantMiddleware;
using ERestaurant.Application.Services.AdditionalMaterialServices;
using ERestaurant.Application.Services.AutoMapperProfiles;
using ERestaurant.Application.Services.ComboMaterialServices;
using ERestaurant.Application.Services.ComboServices;
using ERestaurant.Application.Services.Localization;
using ERestaurant.Application.Services.MaterialServices;
using ERestaurant.Application.Services.Middleware.Interfaces;
using ERestaurant.Application.Services.OrderItemServices;
using ERestaurant.Application.Services.OrderServices;
using ERestaurant.Domain.IUnitOfWork;
using ERestaurant.Domain.Repository.BaseRepository;
using ERestaurant.Infrastructure.DatabaseContext;
using ERestaurant.Infrastructure.HelperClass.Auditing;
using ERestaurant.Infrastructure.HelperClass.DatabaseRecreation;
using ERestaurant.Infrastructure.Repositories;
using ERestaurant.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;
// Add services to the container.

builder.Services.AddControllers()
  .AddJsonOptions(o =>
  {
      o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      o.JsonSerializerOptions.WriteIndented = environment.IsDevelopment();
      o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
  });

builder.Services.AddAppProblemDetails(environment);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithTenantAndLanguage();


#region User Defined Scope
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IComboServices, ComboServices>();
builder.Services.AddScoped<IMaterialServices, MaterialServices>();
builder.Services.AddScoped<OrderServices>()
        .AddScoped<IOrderServices>(sp => sp.GetRequiredService<OrderServices>())
        .AddScoped<IOrderPricingServices>(sp => sp.GetRequiredService<OrderServices>());
builder.Services.AddScoped<IOrderItemServices, OrderItemServices>();
builder.Services.AddScoped<IAdditionalMaterialServices, AdditionalMaterialServices>();
builder.Services.AddScoped<IComboMaterialServices, ComboMaterialServices>();

builder.Services.AddScoped<IRequestTenant, RequestTenant>();
builder.Services.AddScoped<IRequestLanguage, RequestLanguage>();

builder.Services.AddScoped<IAuditStamper, AuditStamper>();
builder.Services.AddDbContext<ERestaurantDbContext>(options =>
{
    options.EnableSensitiveDataLogging().EnableDetailedErrors();
    options.UseLazyLoadingProxies();
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ERestaurant-ConnectionString"]);
});

builder.Services.AddHeaderLocalization();

builder.Services.AddAutoMapper(
    typeof(MaterialProfile).Assembly,
    typeof(ComboMaterialProfile).Assembly,
    typeof(ComboProfile).Assembly,
    typeof(OrderProfile).Assembly,
    typeof(OrderItemProfile).Assembly
    );
#endregion

var app = builder.Build();

await app.UseDevDatabaseRecreateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region User Defined Scope
app.UseAppProblemDetails();

app.UseMiddleware<TenantMiddleware>();

app.UseHeaderLocalization();
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
