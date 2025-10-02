تمام — شلت الأقسام اللي طلبت حذفها. هذا **README المُحدَّث** (جاهز كبديل مباشر):

---

# E-Restaurant Web API

A multi-tenant restaurant backend for menus, combos, orders, and add-ons — with built-in localization (Arabic/English), clean error handling, and a developer-friendly architecture (.NET + EF Core).

> This single README serves **two audiences**:
>
> 1. **Clients / Stakeholders** — overview, capabilities, and how to use the API at a high level.
> 2. **Developers** — detailed setup, architecture, and conventions (the **Developer Guide** is further down).

---

## Table of Contents

* [Client Guide (Overview)](#client-guide-overview)

  * [What is this project?](#what-is-this-project)
  * [Key Features](#key-features)
  * [How to Access](#how-to-access)
  * [Important Headers](#important-headers)
  * [Typical Flows](#typical-flows)
  * [Error Format](#error-format)
  * [Service Levels & Notes](#service-levels--notes)
  * [Contact & Support](#contact--support)
* [Developer Guide (Detailed)](#developer-guide-detailed)

  * [Tech Stack](#tech-stack)
  * [Solution Structure](#solution-structure)
  * [Prerequisites](#prerequisites)
  * [Configuration](#configuration)
  * [Run the API](#run-the-api)
  * [Database & Migrations](#database--migrations)
  * [Seeding](#seeding)
  * [Tenancy Model](#tenancy-model)
  * [Auto-Loading (AutoInclude)](#auto-loading-autoinclude)
  * [Localization](#localization)
  * [Error Handling](#error-handling)
  * [Pagination & Sorting](#pagination--sorting)
  * [API Endpoints (Success Cases)](#api-endpoints-success-cases)
  * [Postman Collection (JSON)](#postman-collection-json)
  * [Testing](#testing)
  * [Coding Standards & Conventions](#coding-standards--conventions)
  * [Troubleshooting](#troubleshooting)

---

## Client Guide (Overview)

### What is this project?

**E-Restaurant Web API** is a backend service for restaurant operations. It powers mobile apps or web dashboards to:

* Browse materials (ingredients/items), combos (meals made of materials), and optional add-ons (e.g., sauces).
* Place and track orders.
* Work across **multiple tenants (restaurants/brands)** cleanly and safely.

The API is **language-aware**:

* Content (like item names) responds in **Arabic** when `X-Accept-Language: ar` is sent, and **English** when `X-Accept-Language: en` is sent.

### Key Features

* **Multi-tenant** isolation with per-tenant data guards.
* **Menu & Combos**: materials, combos, and add-ons.
* **Orders** with order items and add-ons on each item.
* **Localization** at the DTO level (Arabic/English names).
* **Consistent errors** via RFC-7807 `ProblemDetails`.
* **Swagger** for live API documentation.
* **Seed demo data** for quick evaluation.

### How to Access

* Run the API locally (see **Developer Guide**).
* Once running, open: `http://localhost:5000/swagger` (or the port printed in the console) to explore endpoints interactively.

> If you received a hosted/staging URL from us, use that instead of localhost.

### Important Headers

| Header              | Required | Purpose                                                 | Example                 |
| ------------------- | -------- | ------------------------------------------------------- | ----------------------- |
| `X-Tenant-Id`       | ✅ Yes    | Selects which tenant (restaurant) your requests target. | `X-Tenant-Id: 1`        |
| `X-Accept-Language` | Optional | Localizes names in responses. Supports `ar` or `en`.    | `X-Accept-Language: ar` |

### Typical Flows

1. **List materials** (ingredients/items):
   `GET /API/Material`
2. **View combos** (meals composed of materials):
   `GET /API/Combo`
3. **Place an order**:
   `POST /API/Order` with items and optional add-ons.
4. **Get order details**:
   `GET /API/Order/{id}`

> Use **Swagger** to try these with your `X-Tenant-Id` and optional `X-Accept-Language` headers.

### Error Format

Errors follow **ProblemDetails** (RFC-7807). Example:

```json
{
  "type": "https://http.dev/errors/validation",
  "title": "Validation Failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "errors": {
    "Items[0].Quantity": ["Quantity must be at least 1"]
  }
}
```

### Service Levels & Notes

* **Performance**: Optimized queries and auto-loading for common navigation properties.
* **Isolation**: One tenant cannot read/modify another tenant’s data.
* **Localization**: Names switch language based on `X-Accept-Language`.
* **Security**: See Developer Guide for authentication/authorization options (varies by deployment).

### Contact & Support

* For access, demos, or questions, contact your project representative.

---

## Developer Guide (Detailed)

> Everything below is for engineers who will build/extend/deploy the API.

### Tech Stack

* **.NET 8** (or later), ASP.NET Core Web API
* **Entity Framework Core** (SQL Server)
* **AutoMapper** for mapping and localization shaping
* **Hellang.Middleware.ProblemDetails** for consistent error responses
* **Swagger / Swashbuckle** for API docs
* Optional: **JWT/Identity** if authentication is enabled in your deployment

### Solution Structure

Typical layout (adjust to your repo):

```
ERestaurant.sln
src/
  Api/                # Controllers, DI, middlewares, Swagger
  Application/        # DTOs, services, AutoMapper profiles
  Domain/             # Entities, enums, domain logic
  Infrastructure/     # DbContext, EF configs, repositories/UoW, seed
  Testing/            # Unit/integration tests
docs/                 # Diagrams, API docs, etc.
```

### Prerequisites

* .NET 8 SDK or later
* SQL Server (Express/Developer/LocalDB). Ensure **SQL Server Browser** is running for named instances.
* EF Core CLI tools:

  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Configuration

Create **`appsettings.Development.json`** in `src/Api` (or use env vars):

```json
{
  "ConnectionStrings": {
    "ERestaurantDb": "Server=localhost;Database=ERestaurant;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "ErrorHandling": {
    "ExposeExceptionDetails": true
  },
  "Localization": {
    "SupportedCultures": [ "en", "ar" ],
    "DefaultCulture": "en"
  },
  "Tenancy": {
    "Header": "X-Tenant-Id",
    "DefaultTenantId": 1
  },
  "Swagger": {
    "Enable": true
  }
}
```

> **Production**: set a strong SQL connection string, disable `ExposeExceptionDetails`, and restrict Swagger as needed.

### Run the API

From the repo root (adjust paths if different):

```bash
dotnet restore
dotnet build

# Apply migrations then run
dotnet ef database update --project src/Infrastructure --startup-project src/Api
dotnet run --project src/Api
```

Open the printed URL (e.g., `https://localhost:7043/swagger`).

### Database & Migrations

Create a new migration when changing entities/config:

```bash
dotnet ef migrations add <Name> \
  --project src/Infrastructure \
  --startup-project src/Api
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

**Reset DB (Dev only)** — choose one:

* CLI reset:

  ```bash
  dotnet ef database drop -f --project src/Infrastructure --startup-project src/Api
  dotnet ef database update --project src/Infrastructure --startup-project src/Api
  ```
* Or guarded code path (Development only):

  ```csharp
  // in Program.cs during Development
  if (app.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("DevOptions:ResetDatabase"))
  {
      using var scope = app.Services.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
      db.Database.EnsureDeleted();
      db.Database.EnsureCreated();
      SeedData.Apply(scope.ServiceProvider);
  }
  ```

### Seeding

A seed routine creates demo tenants and data, e.g.:

```csharp
Tenants: (1, "Burgers"), (2, "Pizza"), (3, "Shawarma"), (4, "Coffee&Tea"), (5, "Desserts")
```

Materials, combos, and add-ons are generated per tenant with audit fields (`CreatedBy`, `CreatedDate`, etc.).
Enable/disable seeding via config or environment.

### Tenancy Model

* **Header**: `X-Tenant-Id` identifies the active tenant.
* **Isolation**: Entities use `TenantId` and (optionally) alternate keys per entity, e.g.:

  ```csharp
  modelBuilder.Entity<Material>().HasAlternateKey(e => new { e.TenantId, e.Id });
  modelBuilder.Entity<Combo>().HasAlternateKey(e => new { e.TenantId, e.Id });
  modelBuilder.Entity<AdditionalMaterial>().HasAlternateKey(e => new { e.TenantId, e.Id });
  ```
* **Global query filter** applies the current tenant to all queries. A `ITenantProvider` reads the header and stores the `TenantId` for the current request scope.
* **Repository pattern** may stack an extra guard to ensure `TenantId` is always set on inserts/updates.

### Auto-Loading (AutoInclude)

Common navigations are auto-included through a specialized DbContext (or model config):

```csharp
modelBuilder.Entity<Combo>().Navigation(x => x.ComboMaterial).AutoInclude();
modelBuilder.Entity<ComboMaterial>().Navigation(x => x.Material).AutoInclude();

modelBuilder.Entity<Order>().Navigation(x => x.OrderItem).AutoInclude();
modelBuilder.Entity<OrderItem>().Navigation(x => x.Material).AutoInclude();
modelBuilder.Entity<OrderItem>().Navigation(x => x.Combo).AutoInclude();
modelBuilder.Entity<OrderItem>().Navigation(x => x.AdditionalMaterial).AutoInclude();
```

Use this judiciously to balance convenience and performance.

### Localization

* Accepts `X-Accept-Language: ar` or `en`.
* **AutoMapper** projects entities to DTOs using the language flag, e.g. passing an `isArabic` flag into mapping:

  ```csharp
  var dtos = _mapper.ProjectTo<ComboReturnDTO>(query, new { isArabic = lang.IsArabic });
  ```
* Profiles map `Name` from `NameAr` if Arabic, or `NameEn` if English.
* Register `RequestLocalizationOptions` in `Program.cs` with `SupportedCultures` = `[ "en", "ar" ]`.

### Error Handling

* Uses **Hellang.Middleware.ProblemDetails** to map exceptions to standard JSON:

  * `NotFoundException` → 404
  * `ValidationException` → 400 with `errors` dictionary
  * Others → 500 (hide details outside Development)
* Customize in `ProblemDetailsConfig` (maps, links, titles).

### Pagination & Sorting

* Endpoints may return an `X-Pagination` header (JSON) for page/size/total when applicable.
* Common query params:

  * `pageNumber`, `pageSize`
  * `orderBy` (e.g., `CreatedDate`, `OrderDate`, `Name`)
  * `orderByDirection` (`ASC`/`DESC`)

---

## API Endpoints (Success Cases)

> **Only successful/positive flows included below** (negative/error tests from the collection are intentionally omitted).

### Materials

* **GET** `/API/Material`
  List materials (paged/sorted).
  Query: `pageNumber`, `pageSize`, `orderBy`, `orderByDirection`
  Headers: `X-Tenant-Id` (required), `X-Accept-Language` (optional)
* **GET** `/API/Material/{id}`
  Get a single material by id.
  Headers: `X-Tenant-Id`, `X-Accept-Language`
* **POST** `/API/Material`
  Create a material.
  Headers: `X-Tenant-Id`, `Content-Type: application/json`, `X-Accept-Language`
  Body:

  ```json
  {
    "nameEn": "Avocado Slices",
    "nameAr": "شرائح أفوكادو",
    "unit": 1,
    "pricePerUnit": 0.020,
    "tax": 0.160,
    "imageUrl": "https://www.ERestaurant.com/{tenantId}/material/avocado-slices",
    "isActive": true
  }
  ```
* **PUT** `/API/Material/{id}`
  Update a material.
  Headers: `X-Tenant-Id`, `Content-Type: application/json`, `X-Accept-Language`
  Body (example, updating price):

  ```json
  {
    "id": "<materialId>",
    "nameEn": "Cheddar Slice",
    "nameAr": "شريحة شيدر",
    "unit": 0,
    "pricePerUnit": 0.320,
    "tax": 0.160,
    "imageUrl": "https://www.ERestaurant.com/{tenantId}/material/cheddar-slice",
    "isActive": true
  }
  ```

### Combos

* **GET** `/API/Combo`
  List combos (paged/sorted).
  Query: `pageNumber`, `pageSize`, `orderBy`, `orderByDirection`
  Headers: `X-Tenant-Id`, `X-Accept-Language`
* **GET** `/API/Combo/{id}`
  Get a single combo by id.
  Headers: `X-Tenant-Id`, `X-Accept-Language`
* **POST** `/API/Combo`
  Create a combo with materials.
  Headers: `X-Tenant-Id`, `Content-Type: application/json`, `X-Accept-Language`
  Body:

  ```json
  {
    "nameEn": "Spicy Burger Meal",
    "nameAr": "وجبة برجر حارة",
    "price": 5.900,
    "tax": 0.160,
    "imageUrl": "https://www.ERestaurant.com/{tenantId}/combo/spicy-burger-meal",
    "isActive": true,
    "material": [
      { "materialId": "<id>", "quantity": 1 },
      { "materialId": "<id>", "quantity": 20 }
    ]
  }
  ```

#### Combo Materials (sub-resource)

* **POST** `/API/Combo/{comboId}/Material/{materialId}?quantity={n}`
  Add/attach a material to an existing combo.
  Headers: `X-Tenant-Id`, `X-Accept-Language`
* **DELETE** `/API/Combo/{comboId}/Material/{materialId}`
  Remove/detach a material from a combo.
  Headers: `X-Tenant-Id`, `X-Accept-Language`

### Orders

* **GET** `/API/Order`
  List orders (paged/sorted).
  Query: `pageNumber`, `pageSize`, `orderBy`, `orderByDirection`
  Headers: `X-Tenant-Id`, `X-Accept-Language`
* **GET** `/API/Order/{id}`
  Get a single order by id.
  Headers: `X-Tenant-Id`, `X-Accept-Language`
* **POST** `/API/Order`
  Create an order with mixed items (combos, materials, add-ons).
  Headers: `X-Tenant-Id`, `Content-Type: application/json`, `X-Accept-Language`
  Body:

  ```json
  {
    "customerName": "Yousef Q.",
    "customerPhone": "0790000010",
    "orderItem": [
      { "comboId": "<comboId>", "quantity": 1 },
      { "materialId": "<materialId>", "quantity": 150 },
      { "additionalMaterialId": "<addId>", "quantity": 2 }
    ]
  }
  ```

#### Order Items (sub-resource)

* **POST** `/API/Order/{orderId}/Item/Material/{materialId}?quantity={n}`
  Add a material line to an existing order.
  Headers: `X-Tenant-Id`, `X-Accept-Language`
* **DELETE** `/API/Order/{orderId}/Item/OrderItem/{materialId}`
  Remove an order line by reference (e.g., remove fries by `materialId`).
  Headers: `X-Tenant-Id`, `X-Accept-Language`

### Additional Materials (Add-ons)

* **GET** `/API/AdditionalMaterial`
  List add-ons (paged/sorted).
  Query: `pageNumber`, `pageSize`, `orderBy`, `orderByDirection`
  Headers: `X-Tenant-Id`, `X-Accept-Language`
* **GET** `/API/AdditionalMaterial/{id}`
  Get a single add-on by id.
  Headers: `X-Tenant-Id`, `X-Accept-Language`
* **POST** `/API/AdditionalMaterial`
  Create an add-on.
  Headers: `X-Tenant-Id`, `Content-Type: application/json`, `X-Accept-Language`
  Body:

  ```json
  {
    "nameEn": "Spicy Mayo Cup",
    "nameAr": "كوب مايونيز حار",
    "unit": 1,
    "pricePerUnit": 0.008,
    "tax": 0.160,
    "imageUrl": "https://www.ERestaurant.com/{tenantId}/additional-material/spicy-mayo-cup",
    "isActive": true
  }
  ```

> **Notes**
>
> * All write operations are tenant-guarded; IDs must belong to the active tenant.
> * Paged endpoints may include an `X-Pagination` response header with metadata.

---

## Postman Collection (JSON)

A ready-to-import **Postman collection (JSON)** is provided with:

* Predefined **variables** for seeded IDs (materials/combos/orders/add-ons) and **headers**.
* Separate **positive** and **negative** test requests.

  > This README lists only the *positive* (success) flows; the collection also includes negative tests you can run during QA.

**Import tips:**

1. Open Postman → **Import** → paste the JSON or choose the file.
2. Define environment variables as needed (e.g., `baseUrl`, `port`, `tenantId`, `lang`).
3. Ensure your requests include `X-Tenant-Id` and (optionally) `X-Accept-Language`.

---

## Testing

* **Unit tests** for services/mappers.
* **Integration tests** boot the API with in-memory or test SQL DB.
* Seed a minimal dataset per test to avoid cross-test coupling.

## Coding Standards & Conventions

* **C#/.NET Naming**: PascalCase for types/methods, camelCase for locals, `I` prefix for interfaces, DTO suffix `Dto` (or `DTO` consistently).
* **Projects**:

  * `ERestaurant.Api` — web layer only (controllers, DI, middlewares)
  * `ERestaurant.Application` — DTOs, services, AutoMapper
  * `ERestaurant.Domain` — entities/enums/rules
  * `ERestaurant.Infrastructure` — EF Core, repos/UoW, Seed
* **Exception flow**: throw domain/app exceptions; middleware maps to ProblemDetails.

## Troubleshooting

* **Swagger not loading**: ensure `Swagger.Enable` is true for the environment, check HTTPS URLs.
* **SQL connectivity**: verify instance name, `TrustServerCertificate=True` for dev, ensure SQL Browser service is running.
* **AutoMapper mapping issues**: confirm profiles are registered and `isArabic` flag (if used) is passed during `ProjectTo`.
* **Tenant data looks mixed**: confirm `X-Tenant-Id` header is set on every request; verify global query filter is active.
