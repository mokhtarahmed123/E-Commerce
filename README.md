# E‑Commerce Backend

A clean, extensible, production‑ready e‑commerce backend built with **ASP.NET Core**, **Entity Framework Core**, **Clean Architecture**, and **JWT Authentication**. It supports catalog management, customers, carts, orders, payments, and role‑based administration.

> If you find this project useful, please ⭐ the repo!

---

## Table of Contents

* [Features](#features)
* [Tech Stack](#tech-stack)
* [Architecture](#architecture)
* [Domain Highlights](#domain-highlights)
* [Project Structure](#project-structure)
* [Getting Started](#getting-started)

  * [Prerequisites](#prerequisites)
  * [Configuration](#configuration)
  * [Database & Migrations](#database--migrations)
  * [Run the Project](#run-the-project)
  * [Seed Initial Data](#seed-initial-data)
* [API Documentation](#api-documentation)
* [Authentication](#authentication)
* [Development Tips](#development-tips)
* [Testing](#testing)
* [CI/CD (Optional)](#cicd-optional)
* [Roadmap](#roadmap)
* [Contributing](#contributing)
* [License](#license)

---

## Features

* ✅ **User Accounts & Roles**: Admin, Customer (and optional Accountant/Support).
* 🔐 **JWT Authentication** with refresh tokens.
* 🧾 **Catalog**: categories, brands, products, variants, inventory, images.
* 🛒 **Shopping Cart** with discounts and coupons.
* 📦 **Orders**: checkout, order items, status tracking, cancellations/returns.
* 💳 **Payments**: payment intents, capture/void/refund hooks (gateway‑agnostic), transaction logs.
* 🧮 **Pricing**: taxes, shipping, and promotions pipeline.
* 📈 **Reports** (basic): sales by period, top products, customers.
* 🧰 **Admin Panel APIs**: CRUD for catalog, user management, and order operations.
* 🧪 **Unit & Integration Tests** for core services and repositories.
* 🧼 **Clean Architecture** with application & domain separation.

> Note: Some features may be in progress; see [Roadmap](#roadmap).

---

## Tech Stack

* **.NET**: ASP.NET Core 8+ (Web API)
* **Data**: Entity Framework Core, SQL Server (or PostgreSQL)
* **Auth**: ASP.NET Core Identity + JWT
* **Mapping**: AutoMapper
* **Validation**: FluentValidation
* **Logging**: Serilog
* **Docs**: Swagger / OpenAPI
* **Tests**: xUnit / NUnit + Moq

---

## Architecture

Follows a layered/clean architecture for maintainability and testability:

```
E-Commerce.sln
├─ src
│  ├─ ECommerce.Domain          # Entities, Value Objects, Enums, Domain Events
│  ├─ ECommerce.Application     # Use Cases, Services, DTOs, Interfaces, Validators
│  ├─ ECommerce.Infrastructure  # EF Core, Repositories, Identity, External Integrations
│  └─ ECommerce.API             # Controllers, DI, Middlewares, Swagger, Auth
└─ tests
   ├─ ECommerce.UnitTests
   └─ ECommerce.IntegrationTests
```

Key principles:

* **Dependency Inversion**: API → Application → Domain (Infrastructure is a detail).
* **CQRS‑ish Services**: commands/queries via application services.
* **DTOs & Mappers** separate transport from domain models.
* **Domain Events** for side‑effects (e.g., `OrderPlacedEvent`).

---

## Domain Highlights

* **Users** (Identity): `User`, `Role`, claims, refresh tokens
* **Catalog**: `Product`, `ProductVariant`, `Category`, `Brand`, `InventoryItem`
* **Cart**: `Cart`, `CartItem`, coupon codes, discount rules
* **Orders**: `Order`, `OrderItem`, `Shipment`, `Address`, `Payment`
* **Payments**: Gateway abstraction + transaction logs

Typical order flow:

1. Customer registers & logs in.
2. Adds items to cart; applies coupon (optional).
3. Checkout → create order, reserve stock, create payment intent.
4. On payment success → capture, mark order `Paid`, reduce inventory, emit `OrderPaidEvent`.

---

## Project Structure

> Actual namespaces may vary—adjust examples to your code.

```
src/
  ECommerce.API/
    Controllers/
      AuthController.cs
      ProductsController.cs
      CartController.cs
      OrdersController.cs
      PaymentsController.cs
    Middlewares/
    Program.cs

  ECommerce.Application/
    DTOs/
    Interfaces/
    Services/
    Validators/
    Mapping/

  ECommerce.Domain/
    Entities/
    ValueObjects/
    Enums/
    Events/

  ECommerce.Infrastructure/
    Persistence/
      ApplicationDbContext.cs
      Migrations/
    Identity/
    Repositories/
    Configurations/
```

---

## Getting Started

### Prerequisites

* [.NET SDK 8+](https://dotnet.microsoft.com/)
* SQL Server (LocalDB or Docker) *or* PostgreSQL
* (Optional) Docker & Docker Compose

### Configuration

Create **`appsettings.Development.json`** in `ECommerce.API`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Issuer": "ECommerce",
    "Audience": "ECommerce.Client",
    "Key": "REPLACE_WITH_LONG_RANDOM_SECRET",
    "AccessTokenMinutes": 30,
    "RefreshTokenDays": 7
  },
  "Serilog": {
    "MinimumLevel": "Information"
  },
  "Swagger": {
    "Enabled": true
  }
}
```

> For PostgreSQL: `Host=localhost;Database=ECommerceDb;Username=postgres;Password=yourpw` and switch the EF provider in `Infrastructure`.

### Database & Migrations

From the solution root:

```bash
# Restore & build
dotnet restore
dotnet build

# Create database (if needed) and run migrations
cd src/ECommerce.API
# If migrations do not exist yet
# dotnet ef migrations add InitialCreate -p ../ECommerce.Infrastructure -s .

dotnet ef database update -p ../ECommerce.Infrastructure -s .
```

### Run the Project

```bash
dotnet run --project src/ECommerce.API
```

API will start on `https://localhost:5001` (or shown in console).

### Seed Initial Data

Add a simple seeder (example):

```csharp
// Program.cs (inside a scoped service)
await DatabaseSeeder.SeedAsync(app.Services);
```

Seeder can create:

* Admin user (`admin@shop.com` / `Admin@123`)
* Sample categories/products/brands
* Test coupon codes

---

## API Documentation

* Swagger UI available at `/swagger` in Development.
* Exportable OpenAPI spec at `/swagger/v1/swagger.json`.

### Common Endpoints (examples)

* `POST /api/auth/register`
* `POST /api/auth/login`
* `POST /api/auth/refresh`
* `GET /api/products`
* `POST /api/cart/items`
* `POST /api/orders`
* `POST /api/payments/intent`

> Check controllers for the full list and request/response DTOs.

---

## Authentication

Use `Authorization: Bearer <access_token>` header for secured endpoints.

Login flow:

1. `POST /api/auth/login` → returns access & refresh tokens.
2. Use access token for API calls.
3. When expired, call `POST /api/auth/refresh` with refresh token.

Roles & policies (examples):

* `Admin` can manage catalog/users/orders.
* `Customer` can manage own cart/orders.

---

## Development Tips

* **Validation** via FluentValidation in `Application` layer.
* **Mapping** profiles in `Application.Mapping` (AutoMapper).
* **Global Error Handling** middleware with problem details.
* **Soft Deletes** for catalog entities (optional).
* **Specification/Query Objects** for complex queries (pagination, filters, sorting).

---

## Testing

```bash
# Run all tests
dotnet test
```

* Unit tests for services and domain logic
* Integration tests with in‑memory or Testcontainers DB

---

## CI/CD (Optional)

Example GitHub Actions workflow (summarized):

```yaml
name: ci
on: [push, pull_request]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - run: dotnet restore
      - run: dotnet build --no-restore --configuration Release
      - run: dotnet test --no-build --configuration Release
```

---

## Roadmap

* [ ] Advanced promotions/discount engine
* [ ] Webhooks for payment gateways (Stripe/PayMob/etc.)
* [ ] Inventory reservations & backorders
* \[
<img width="528" height="455" alt="Screenshot 2025-07-27 070039" src="https://github.com/user-attachments/assets/1416c28b-564c-4ce8-aea0-b46dfb569032" />
<img width="579" height="360" alt="Screenshot 2025-07-27 070056" src="https://github.com/user-attachments/assets/14cfe39c-f522-4804-87ef-b01ccff640bd" />
<img width="1396" height="636" alt="Screenshot 2025-07-27 070136" src="https://github.com/user-attachments/assets/2e1337aa-87e8-4c83-b721-b50d7c6f0915" />
<img width="1400" height="373" alt="Screenshot 2025-07-27 070155" src="https://github.com/user-attachments/assets/c671236b-3d9f-4bc2-89b7-0e31b29ed3d6" />
<img width="1514" height="380" alt="Screenshot 2025-07-27 070207" src="https://github.com/user-attachments/assets/afb3a440-1565-4d2f-a4e6-af797cd3b0b5" />
<img width="1496" height="623" alt="Screenshot 2025-07-27 070230" src="https://github.com/user-attachments/assets/27506e78-a9a7-45c0-9404-c9d38b44a82d" />
<img width="569" height="513" alt="Screenshot 2025-07-27 070502" src="https://github.com/user-attachments/assets/a656ea23-bcbf-4183-bad4-0662615d9721" />
<img width="1128" height="249" alt="Screenshot 2025-07-27 070519" src="https://github.com/user-attachments/assets/07712eec-3c34-429b-b2a2-de312fac0dbe" />

<img width="1254" height="349" alt="Screenshot 2025-07-27 070535" src="https://github.com/user-attachments/assets/c501b71b-15e1-4585-8786-d6fe9bc99371" />


