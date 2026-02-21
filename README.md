# TeamTasks

A microservice-based task management backend built to learn ASP.NET Core, SQL Server, DbUp, and Docker.

## Overview

TeamTasks is composed of three independent services that communicate over HTTP:

| Service | Port | Responsibility |
|---|---|---|
| **User Service** | `5001` | Manage users (CRUD) |
| **Task Service** | `5002` | Manage tasks, validate assigned users |
| **Notification Service** | `5003` | Create and list notifications |

Each service owns its own SQL Server database and runs its own DbUp migrations on startup.

---

## Project Structure

```
teamtasks/
├── docker-compose.yml                  # Local dev orchestration
├── services/
│   ├── user-service/
│   │   ├── Dockerfile
│   │   ├── UserService.slnx
│   │   ├── UserService.API/
│   │   │   ├── Controllers/
│   │   │   │   └── UsersController.cs
│   │   │   ├── Migrations/
│   │   │   │   └── 001_create_users.sql
│   │   │   ├── Program.cs
│   │   │   ├── appsettings.json
│   │   │   └── appsettings.Development.json  # gitignored
│   │   └── UserService.Tests/
│   ├── task-service/
│   │   ├── Dockerfile
│   │   ├── TaskService.API/
│   │   │   ├── Controllers/
│   │   │   │   └── TasksController.cs
│   │   │   ├── Migrations/
│   │   │   │   └── 001_create_tasks.sql
│   │   │   ├── Program.cs
│   │   │   ├── appsettings.json
│   │   │   └── appsettings.Development.json  # gitignored
│   └── notification-service/
│       ├── Dockerfile
│       ├── NotificationService.API/
│       │   ├── Controllers/
│       │   │   └── NotificationsController.cs
│       │   ├── Migrations/
│       │   │   └── 001_create_notifications.sql
│       │   ├── Program.cs
│       │   ├── appsettings.json
│       │   └── appsettings.Development.json  # gitignored
└── .github/
    └── workflows/                      # CI/CD pipelines
```

---

## API Endpoints

### User Service — `http://localhost:5001`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/users` | Get all users |
| `GET` | `/api/users/{id}` | Get user by ID |
| `POST` | `/api/users` | Create a new user |

**POST /api/users** request body:
```json
{ "name": "Alice", "email": "alice@example.com" }
```

---

### Task Service — `http://localhost:5002`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/tasks` | Get all tasks |
| `POST` | `/api/tasks` | Create a task (validates assigned user) |

**POST /api/tasks** request body:
```json
{ "title": "Fix login bug", "assignedUserId": 1 }
```

> ⚠️ Task creation calls the User Service internally to verify the assigned user exists. The user-service must be running.

---

### Notification Service — `http://localhost:5003`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/notifications` | Get all notifications |
| `POST` | `/api/notifications` | Create a notification |

**POST /api/notifications** request body:
```json
{ "message": "Task assigned to you." }
```

---

## Tech Stack

- **Runtime**: .NET 10, ASP.NET Core
- **Database**: SQL Server 2022
- **Migrations**: [DbUp](https://dbup.readthedocs.io/) — embedded SQL scripts run on startup
- **API Docs**: Swagger UI (available at `/swagger` on each service)
- **Containerisation**: Docker + Docker Compose

---

## Running Locally with Docker

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Start all services

```bash
docker compose up --build
```

This will:
1. Start SQL Server 2022 on port `1433`
2. Build and start all three services
3. Each service auto-runs its DbUp migrations on startup

### Stop everything

```bash
docker compose down
```

To also remove the database volume (full reset):

```bash
docker compose down -v
```

### Access Swagger UI

| Service | Swagger URL |
|---|---|
| User Service | http://localhost:5001/swagger |
| Task Service | http://localhost:5002/swagger |
| Notification Service | http://localhost:5003/swagger |

---

## Running a Service Locally (Without Docker)

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server running locally (or via Docker: `docker compose up sql`)

### Setup

Each service has an `appsettings.Development.json` (gitignored) for local secrets. Create it manually if missing:

**`services/user-service/UserService.API/appsettings.Development.json`**
```json
{
  "ConnectionStrings": {
    "UsersDb": "Server=localhost,1433;Database=users_dev;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True"
  }
}
```

Do the same for `task-service` (`TasksDb`) and `notification-service` (`NotificationsDb`). The task-service also needs:
```json
{
  "ServiceUrls": {
    "UserService": "http://localhost:5001"
  }
}
```

### Run

```bash
cd services/user-service/UserService.API
dotnet run
```

---

## Database Migrations

Migrations are plain `.sql` files under each service's `Migrations/` folder, embedded into the build output and run automatically by DbUp on startup. They are applied in filename order and are **idempotent** (each script runs only once, tracked in a `SchemaVersions` table).

To add a new migration, create a new file following the naming convention:

```
002_add_column_to_users.sql
```

Ensure the file has **Build Action: EmbeddedResource** (already configured via the `.csproj` glob `Migrations/**/*.sql`).

---

## Configuration

Config is layered using ASP.NET Core's built-in system:

| File | Committed | Purpose |
|---|---|---|
| `appsettings.json` | ✅ Yes | Base config with placeholder keys |
| `appsettings.Development.json` | ❌ No (gitignored) | Local dev secrets |
| Environment variables | — | Used in Docker Compose / production |

Connection strings in Docker Compose are injected as environment variables using `__` (double-underscore) as the section separator, e.g.:

```
ConnectionStrings__UsersDb=Server=sql,...
```

---

## Contributing

1. Fork the repo and create a feature branch
2. Follow the existing service structure when adding new services
3. Add DbUp migration scripts for any schema changes
4. Open a pull request with a clear description
