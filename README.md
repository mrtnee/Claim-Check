# ClaimCheck

A propaganda debunking tool where users submit a claim or talking point, the app analyzes it via AI, and returns structured results identifying propaganda techniques, counter-arguments, and a truthfulness assessment.

## Architecture

Layered architecture (DDD-inspired, not over-engineered):

- **Domain** — core entities and business logic
- **Application** — use cases and orchestration
- **Infrastructure** — EF Core, Anthropic HTTP client, PostgreSQL
- **API** — ASP.NET Core Web API
- **Web** — Blazor frontend with MudBlazor

## Tech Stack

| Concern | Technology |
|---|---|
| Backend | ASP.NET Core Web API (C#) |
| Frontend | Blazor + MudBlazor |
| Database | PostgreSQL via EF Core |
| LLM | Anthropic Claude API (raw HTTP, no SDK) |
| Auth | ASP.NET Core Identity + JWT |
| Testing | xUnit + Moq |
| Containerization | Docker |
| CI/CD | GitHub Actions |
| Hosting | AWS ECS Fargate (API), RDS (PostgreSQL), S3 + CloudFront (frontend) |

## Features

**Core**
- Submit a claim for analysis
- Receive structured debunking: propaganda techniques, counter-arguments, truthfulness assessment
- Persist results to database
- User history

**Nice to have**
- Favorite/save debunkings
- Search past results
- Shareable public links
- Caching for repeated claims
- Per-user rate limiting

## LLM Integration

Calls the Anthropic API directly via a thin `HttpClient` wrapper. Each request sends a system prompt defining the analysis JSON schema and a user message containing the claim. Structured outputs enforce the response format.

## Getting Started

### Prerequisites

- .NET 10 SDK
- PostgreSQL
- Docker

### Running with Docker Compose (recommended)

The easiest way to run the full stack locally — API, frontend, and database all start with a single command.

1. Copy the example env files:
   ```bash
   cp .env.example .env
   cp .env.api.example .env.api
   ```
2. Fill in the required secrets in `.env.api`:
   - `ANTHROPIC__APIKEY` — your Anthropic API key
   - `JWT__KEY` — any string of at least 32 characters
3. Start all services:
   ```bash
   docker compose up --build
   ```
4. Access the app:
   - Frontend: http://localhost:5138
   - API: http://localhost:5278

Database migrations run automatically on API startup — no manual steps needed.

---

### Running locally (without Docker)

1. Clone the repository
2. Configure your connection string and Anthropic API key in `appsettings.Development.json`
3. Apply EF Core migrations:
   ```bash
   dotnet ef database update --project src/ClaimCheck.Infrastructure
   ```
4. Run the API:
   ```bash
   dotnet run --project src/ClaimCheck.API
   ```
5. Run the frontend:
   ```bash
   dotnet run --project src/ClaimCheck.Web
   ```

## Testing

```bash
dotnet test
```

Unit tests cover the domain and application layers. Integration tests cover the Anthropic client and database layer.
