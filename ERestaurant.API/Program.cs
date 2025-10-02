using ERestaurant.API.Middlewares.GlobalExceptionMiddleware;
using ERestaurant.API.Middlewares.LanguageMiddleware;
using ERestaurant.API.Middlewares.SwaggerService;
using ERestaurant.API.Middlewares.TenantMiddleware;
using ERestaurant.Application.AdditionalMaterials.AdditionalMaterialServices;
using ERestaurant.Application.ComboMaterials.AutoMapperProfiles;
using ERestaurant.Application.ComboMaterials.ComboMaterialServices;
using ERestaurant.Application.Combos.AutoMapperProfiles;
using ERestaurant.Application.Combos.ComboServices;
using ERestaurant.Application.Materials.AutoMapperProfiles;
using ERestaurant.Application.Materials.MaterialServices;
using ERestaurant.Application.OrderItems.AutoMapperProfiles;
using ERestaurant.Application.OrderItems.OrderItemServices;
using ERestaurant.Application.Orders.AutoMapperProfiles;
using ERestaurant.Application.Orders.OrderServices;
using ERestaurant.Application.Services.Localization;
using ERestaurant.Application.Services.MiddlewareInterfaces;
using ERestaurant.Domain.IUnitOfWork;
using ERestaurant.Domain.Repository.BaseRepository;
using ERestaurant.Infrastructure.DatabaseContext;
using ERestaurant.Infrastructure.HelperClass.Auditing;
using ERestaurant.Infrastructure.HelperClass.DatabaseRecreation;
using ERestaurant.Infrastructure.Repositories;
using ERestaurant.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddScoped<IComboService, ComboService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<OrderService>()
        .AddScoped<IOrderService>(sp => sp.GetRequiredService<OrderService>())
        .AddScoped<IOrderPricingService>(sp => sp.GetRequiredService<OrderService>());
builder.Services.AddScoped<IOrderItemServices, OrderItemServices>();
builder.Services.AddScoped<IAdditionalMaterialService, AdditionalMaterialService>();
builder.Services.AddScoped<IComboMaterialService, ComboMaterialService>();

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

app.UseMiddleware<TenantMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region User Defined Scope
app.UseAppProblemDetails();

app.UseHeaderLocalization();
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
