# RaceDay.API

RaceDay.API is a backend web API for managing race events, participants, and results. It provides endpoints for creating and managing races, registering participants, recording results, and retrieving summaries and leaderboards.

> NOTE: This README is a general template. Replace example commands, environment variables, and endpoint paths with the concrete details from your project as needed.

## Table of contents

- [Features](#features)
- [Tech stack](#tech-stack)
- [Requirements](#requirements)
- [Getting started](#getting-started)
  - [Clone the repo](#clone-the-repo)
  - [Configuration](#configuration)
  - [Local development](#local-development)
- [Database migrations](#database-migrations)
- [Running with Docker](#running-with-docker)
- [API overview](#api-overview)
  - [Authentication](#authentication)
  - [Common endpoints](#common-endpoints)
  - [Example requests](#example-requests)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- Create and manage race events
- Register participants
- Record race results and compute standings
- Search and filter races and participants
- Authentication and authorization (JWT or other strategies)
- OpenAPI / Swagger documentation (if present)

## Tech stack

- .NET (ASP.NET Core Web API)
- Entity Framework Core (or your chosen ORM)
- SQL Server / PostgreSQL / SQLite (replace with your database)
- Docker (optional)

## Requirements

- .NET SDK 6.0+ (or the version the project targets)
- A supported database (SQL Server, PostgreSQL, or SQLite)
- (Optional) Docker & Docker Compose

## Getting started

### Clone the repo

```bash
git clone https://github.com/MSCHUNLI23/RaceDay.API.git
cd RaceDay.API
```

### Configuration

Create an `appsettings.Development.json` or set environment variables for connection strings and secrets. Example `appsettings.json` keys the project might expect:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RaceDayDb;User Id=sa;Password=Your_password123;"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "RaceDay",
    "Audience": "RaceDayUsers"
  }
}
```

Set environment variables as needed (example):

```bash
export ConnectionStrings__DefaultConnection="Server=localhost;Database=RaceDayDb;User Id=sa;Password=Your_password123;"
export Jwt__Key="your-secret-key"
```

Adjust keys and names to match the code in this repository.

### Local development

Restore packages and run the API:

```bash
# restore
dotnet restore

# run migrations (see Database migrations section)
# run
dotnet run --project src/YourApiProject
```

Replace `src/YourApiProject` with the actual project path if different.

## Database migrations

If this project uses EF Core migrations, apply them with:

```bash
# from the project containing the DbContext
dotnet ef database update
```

To add a migration:

```bash
dotnet ef migrations add AddSomeFeature
dotnet ef database update
```

If your project uses another migration system, consult its documentation and update this section accordingly.

## Running with Docker

A sample Dockerfile and docker-compose setup makes running the API and a database simple.

Build and run with Docker:

```bash
# build
docker build -t raceday-api .

# run
docker run -e ConnectionStrings__DefaultConnection="Server=db;Database=RaceDayDb;User Id=sa;Password=Your_password123;" -p 5000:80 raceday-api
```

Or with docker-compose (example):

```yaml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "5000:80"
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Database=RaceDayDb;User Id=sa;Password=Your_password123;
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      SA_PASSWORD: "Your_password123"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
```

Again, update the examples to match your project's actual container and connection settings.

## API overview

If your project includes Swagger / OpenAPI, you can visit `/swagger` (or the configured path) to browse the API and try endpoints.

### Authentication

This API commonly uses JWT for authentication. Obtain a token by calling the auth endpoint (if implemented) and include it in requests:

```
Authorization: Bearer <token>
```

### Common endpoints (examples)

- GET /api/races - list races
- POST /api/races - create a race
- GET /api/races/{id} - get race details
- POST /api/races/{id}/participants - register a participant
- POST /api/races/{id}/results - submit results
- GET /api/races/{id}/leaderboard - get standings

Replace the paths above with the actual routes in this repository.

### Example requests

Get races:

```bash
curl -X GET "http://localhost:5000/api/races" -H "Accept: application/json"
```

Create a race (authenticated):

```bash
curl -X POST "http://localhost:5000/api/races" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"name": "City 10K", "date": "2026-12-01"}'
```

## Testing

If there are unit or integration tests, run them with:

```bash
dotnet test
```

For integration tests that require a database, prefer using an in-memory provider or a test container.

## Contributing

Contributions are welcome. Please follow these steps:

1. Fork the repository
2. Create a feature branch
3. Run tests and ensure linting/style checks pass
4. Open a pull request describing your change

Include any contribution guidelines, code style, or commit message conventions used in this repo.

## License

This repository does not include a license file by default. Add a LICENSE file (e.g., MIT) to make the licensing explicit.

## Contact

If you have questions, open an issue or contact the repository owner.
