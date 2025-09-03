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
<img width="1886" height="911" alt="Screenshot 2025-09-04 004407" src="https://github.com/user-attachments/assets/0dd00abf-d48d-4449-954b-18650c610705" />
<img width="1884" height="950" alt="Screenshot 2025-09-04 004517" src="https://github.com/user-attachments/assets/3717de73-c52e-4887-ac82-00e18d0de5f2" />
<img width="1910" height="934" alt="Screenshot 2025-09-04 004536" src="https://github.com/user-attachments/assets/b6ff0e92-d3b6-49a8-8be7-98f3df1ead91" />
<img width="1919" height="934" alt="Screenshot 2025-09-04 004703" src="https://github.com/user-attachments/assets/5a024410-79ba-405d-8456-c57368f6a775" />
<img width="1903" height="947" alt="Screenshot 2025-09-04 004726" src="https://github.com/user-attachments/assets/ea4affda-2ab4-44de-872f-81dc08e9b345" />
<img width="1911" height="949" alt="Screenshot 2025-09-04 004747" src="https://github.com/user-attachments/assets/8d8c8de1-d35f-4bff-82a9-ba644cd1f331" />
<img width="1914" height="929" alt="Screenshot 2025-09-04 004815" src="https://github.com/user-attachments/assets/72cbb081-4283-4c51-a0bc-8b12b57e1e7f" />
<img width="1915" height="930" alt="Screenshot 2025-09-04 004835" src="https://github.com/user-attachments/assets/2dd0512c-90b2-4c88-9b92-a769ad40db46" />
<img width="1909" height="945" alt="Screenshot 2025-09-04 004901" src="https://github.com/user-attachments/assets/89a61003-5fd7-418d-bbed-8ff9ccccf300" />
<img width="1911" height="953" alt="Screenshot 2025-09-04 004925" src="https://github.com/user-attachments/assets/cad7380b-1668-4b1c-99c6-1f295499b6b5" />
<img width="1918" height="947" alt="Screenshot 2025-09-04 005014" src="https://github.com/user-attachments/assets/f06f66ab-51af-4a72-bbc9-a05ded0221b1" />
<img width="1882" height="955" alt="Screenshot 2025-09-04 005036" src="https://github.com/user-attachments/assets/1dd080ca-f6b4-4125-b984-50d7752d7fa3" />
<img width="1887" height="937" alt="Screenshot 2025-09-04 005053" src="https://github.com/user-attachments/assets/c8caf10d-1470-46ff-9960-10f2db18558c" />
<img width="1913" height="951" alt="Screenshot 2025-09-04 005119" src="https://github.com/user-attachments/assets/cd054402-3db8-4a8f-86f2-ebf13f4a13ec" />
<img width="1919" height="949" alt="Screenshot 2025-09-04 005159" src="https://github.com/user-attachments/assets/39157f8c-58c0-406f-a416-e0bb13d45733" />




