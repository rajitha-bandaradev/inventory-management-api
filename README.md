# Inventory Management API

A production-style RESTful API for inventory management, built with **ASP.NET Core 10** and **Clean Architecture** principles. Demonstrates layered design, testability, CI/CD, and containerisation.

<!-- TODO: Add a screenshot of Swagger UI here once endpoints are running -->
<!-- ![Swagger UI](docs/images/swagger.png) -->

## Features

- Product & category CRUD with validation (FluentValidation)
- Stock level tracking with low-stock alerts
- JWT authentication & role-based authorisation
- Pagination, filtering, and sorting on list endpoints
- Global error handling middleware with problem-details responses
- Unit tests with xUnit + Moq
- CI pipeline via GitHub Actions
- Docker support (multi-stage build)

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 (Web API) |
| ORM | Entity Framework Core 8 |
| Database | SQLite (dev) — swappable via repository pattern |
| Auth | JWT Bearer tokens |
| Validation | FluentValidation |
| Testing | xUnit, Moq |
| CI/CD | GitHub Actions |
| Containerisation | Docker |

## Architecture

Clean Architecture with strict dependency direction — outer layers depend on inner, never the reverse:

```
┌─────────────────────────────────────┐
│  Api (Controllers, Middleware)      │
│  ┌───────────────────────────────┐  │
│  │  Infrastructure (EF Core,     │  │
│  │  Repositories, SQLite)        │  │
│  │  ┌─────────────────────────┐  │  │
│  │  │  Application (Services, │  │  │
│  │  │  DTOs, Interfaces)      │  │  │
│  │  │  ┌───────────────────┐  │  │  │
│  │  │  │  Domain (Entities)│  │  │  │
│  │  │  └───────────────────┘  │  │  │
│  │  └─────────────────────────┘  │  │
│  └───────────────────────────────┘  │
└─────────────────────────────────────┘
```

**Why Clean Architecture?** Business rules live in the core with zero framework dependencies, so the database, web framework, or UI can change without touching domain logic. This mirrors patterns used in large enterprise systems.

## Project Structure

```
src/
  InventoryApi.Domain/          Entities and domain logic — no external dependencies
  InventoryApi.Application/     Use cases, DTOs, service interfaces
  InventoryApi.Infrastructure/  EF Core DbContext, repository implementations
  InventoryApi.Api/             Controllers, middleware, DI configuration
tests/
  InventoryApi.Tests/           Unit tests (xUnit + Moq)
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run locally

```bash
git clone https://github.com/rajitha-bandaradev/inventory-management-api.git
cd inventory-management-api
dotnet restore
dotnet ef database update --project src/InventoryApi.Infrastructure --startup-project src/InventoryApi.Api
dotnet run --project src/InventoryApi.Api
```

API available at `https://localhost:5001` — Swagger UI at `/swagger`.

### Run tests

```bash
dotnet test
```

### Run with Docker

```bash
docker build -t inventory-api .
docker run -p 8080:8080 inventory-api
```

## API Endpoints

<!-- TODO: Update this table as you build each endpoint -->

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/products` | List products (paged, filterable) | — |
| GET | `/api/products/{id}` | Get product by ID | — |
| POST | `/api/products` | Create product | Admin |
| PUT | `/api/products/{id}` | Update product | Admin |
| DELETE | `/api/products/{id}` | Delete product | Admin |
| POST | `/api/auth/login` | Get JWT token | — |

## Roadmap

- [x] Clean Architecture solution scaffold
- [ ] Product & category CRUD
- [ ] FluentValidation rules
- [ ] Unit tests (xUnit + Moq)
- [ ] JWT authentication
- [ ] GitHub Actions CI
- [ ] Dockerfile + docker-compose
- [ ] Azure App Service deployment

## About Me

Senior .NET Engineer with 9+ years of enterprise experience, including sole ownership of a production Excel VSTO add-in serving 500+ users. This project demonstrates modern backend architecture beyond Office development.

- GitHub: [rajitha-bandaradev](https://github.com/rajitha-bandaradev)
- LinkedIn: [linkedin.com/in/rajitha-bandaradev](https://linkedin.com/in/rajitha-bandaradev)
- Email: rajitha.bandaradev@gmail.com

## License

MIT
