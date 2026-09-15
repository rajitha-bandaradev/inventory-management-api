![CI](https://github.com/rajitha-bandaradev/inventory-management-api/actions/workflows/ci.yml/badge.svg)
# Inventory Management API

A production-style RESTful API for inventory management, built with **ASP.NET Core 10** and **Clean Architecture** principles. Demonstrates layered design, testability, and CI/CD.

## Features

- Product CRUD (GET, POST, PUT, DELETE)
- Request validation with FluentValidation, returning problem-details responses
- Stock level tracking with a low-stock endpoint driven by domain logic
- EF Core persistence with migrations applied automatically on startup
- Unit tests with xUnit + Moq
- CI pipeline via GitHub Actions — build and test on every push

### Planned

- JWT authentication & role-based authorisation
- Category CRUD
- Pagination, filtering, and sorting on list endpoints
- Global error handling middleware
- Docker support (multi-stage build)
- Azure App Service deployment with Azure SQL

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 (Minimal APIs) |
| ORM | Entity Framework Core 10 |
| Database | SQLite (dev) — swappable via repository pattern |
| Validation | FluentValidation |
| Testing | xUnit, Moq |
| CI/CD | GitHub Actions |

## Architecture

Clean Architecture with strict dependency direction — outer layers depend on inner, never the reverse:

```
┌─────────────────────────────────────┐
│  Api (Endpoints, DI)                │
│  ┌───────────────────────────────┐  │
│  │  Infrastructure (EF Core,     │  │
│  │  Repositories, SQLite)        │  │
│  │  ┌─────────────────────────┐  │  │
│  │  │  Application (Services, │  │  │
│  │  │  Validators, Interfaces)│  │  │
│  │  │  ┌───────────────────┐  │  │  │
│  │  │  │  Domain (Entities)│  │  │  │
│  │  │  └───────────────────┘  │  │  │
│  │  └─────────────────────────┘  │  │
│  └───────────────────────────────┘  │
└─────────────────────────────────────┘
```

**Why Clean Architecture?** Business rules live in the core with zero framework dependencies, so the database, web framework, or UI can change without touching domain logic. The repository interface lives in Application; its EF Core implementation lives in Infrastructure — dependency inversion in practice, and what makes the service layer unit-testable with mocks.

## Project Structure

```
src/
  InventoryApi.Domain/          Entities and domain logic — no external dependencies
  InventoryApi.Application/     Services, validators, repository interfaces
  InventoryApi.Infrastructure/  EF Core DbContext, repository implementations, migrations
  InventoryApi.Api/             Endpoint definitions and DI configuration
tests/
  InventoryApi.Tests/           Unit tests (xUnit + Moq)
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run locally

```bash
git clone https://github.com/rajitha-bandaradev/inventory-management-api.git
cd inventory-management-api
dotnet restore
dotnet run --project src/InventoryApi.Api
```

The database is created and seeded automatically on first run. The API listens on `http://localhost:5262` (and `https://localhost:7038`); the OpenAPI document is served at `/openapi/v1.json`.

Sample requests for every endpoint are in `src/InventoryApi.Api/InventoryApi.Api.http` — open it in Visual Studio or VS Code and send them directly.

### Run tests

```bash
dotnet test
```

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/products` | List all products |
| GET | `/api/products/{id}` | Get product by ID |
| GET | `/api/products/low-stock` | Products at or below their reorder level |
| POST | `/api/products` | Create a product (validated) |
| PUT | `/api/products/{id}` | Update a product (validated) |
| DELETE | `/api/products/{id}` | Delete a product |

Writes are validated before they reach the database: invalid payloads return `400` with per-field messages, and requests for a missing product return `404`.

## Roadmap

- [x] Clean Architecture solution scaffold
- [x] Product CRUD (GET, POST, PUT, DELETE)
- [x] FluentValidation rules
- [x] Unit tests (xUnit + Moq)
- [x] GitHub Actions CI
- [ ] JWT authentication
- [ ] Dockerfile + docker-compose
- [ ] Azure App Service deployment

## About Me

Senior .NET Engineer with 9+ years of enterprise experience, including sole ownership of a production Excel VSTO add-in serving 500+ users. This project demonstrates modern backend architecture beyond Office development.

- GitHub: [rajitha-bandaradev](https://github.com/rajitha-bandaradev)
- LinkedIn: [linkedin.com/in/rajitha-bandaradev](https://linkedin.com/in/rajitha-bandaradev)
- Email: rajitha.bandaradev@gmail.com

## License

MIT
