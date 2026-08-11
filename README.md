# Gym Management API

Tested ASP.NET Core 8 Web API for gym operations, member services, training plans, nutrition, and identity workflows.

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![API](https://img.shields.io/badge/HTTP_actions-64-blue)](#api-modules)
[![Tests](https://img.shields.io/badge/tests-106%20passing-brightgreen)](#automated-tests)
[![SQL Server](https://img.shields.io/badge/SQL_Server-EF_Core-CC2927?logo=microsoftsqlserver)](https://learn.microsoft.com/ef/core/)

## Overview

Gym Management API is an independent backend project covering the main workflows of a gym: memberships, attendance, classes, workout and nutrition plans, meals, exercises, BMI records, feedback, and notifications.

The project emphasizes explicit application mapping, secure token handling, soft-delete behavior, paginated queries, structured logging, and automated testing. Its 14 controllers currently expose **64 HTTP actions**.

## Main Features

- ASP.NET Core Identity with role-based authorization.
- JWT access tokens and rotating refresh tokens.
- Registration, email confirmation, password changes, OTP reset, token refresh, and revocation.
- Membership, attendance, class, exercise, workout, meal, and nutrition-plan management.
- BMI history and category calculation.
- Feedback and member notification workflows.
- Cloudinary image uploads and SMTP email delivery.
- Soft delete through an EF Core interceptor and global query filters.
- Pagination with guarded page sizes, no-tracking reads, and lookup indexes.
- Serilog console and rolling-file logs.
- Swagger/OpenAPI with Bearer-token support.

## Architecture

This repository uses a pragmatic N-Tier architecture rather than claiming strict Clean Architecture:

```text
GymMangamentSystem
    API controllers, middleware, configuration, Swagger and composition root
        |
GymMangamentSystem.Core
    Entities, DTOs, enums, interfaces and shared contracts
        |
GymMangamentSystem.Reposatory
    EF Core, repositories, auth services, mapping, migrations and interceptors

GymMangamentSystem.UnitTests
GymMangamentSystem.IntegrationTests
```

## Tech Stack

| Area | Technology |
| --- | --- |
| Runtime | .NET 8, ASP.NET Core Web API |
| Persistence | Entity Framework Core, SQL Server |
| Identity | ASP.NET Core Identity, JWT, rotating refresh tokens, RBAC |
| Observability | Serilog console and rolling files |
| Integrations | MailKit/SMTP, Otp.NET, Cloudinary |
| API docs | Swagger/OpenAPI |
| Testing | xUnit, SQL Server LocalDB integration tests, coverlet collector |

## Project Structure

```text
GymMangamentSystem/
|-- GymMangamentSystem/                  # API host and controllers
|-- GymMangamentSystem.Core/             # Domain models and contracts
|-- GymMangamentSystem.Reposatory/       # Persistence and service implementations
|-- GymMangamentSystem.UnitTests/        # Isolated unit tests
|-- GymMangamentSystem.IntegrationTests/ # SQL Server integration tests
`-- GymMangamentSystem.sln
```

The `Reposatory` spelling is retained because it is part of the existing solution and project names.

## Getting Started

### Requirements

- .NET 8 SDK
- SQL Server
- SQL Server LocalDB on Windows to run the current integration suite
- SMTP credentials for email features
- Cloudinary credentials for image features
- EF Core CLI tools

### 1. Clone and restore

```powershell
git clone https://github.com/uosefahmed22/GymMangamentSystem.git
cd GymMangamentSystem
dotnet restore
```

### 2. Configure local secrets

Safe placeholders and non-secret options are documented in `GymMangamentSystem/appsettings.Example.json`. Configure sensitive values with .NET User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnections" "<connection-string>" --project GymMangamentSystem
dotnet user-secrets set "JWT:Key" "<at-least-32-character-secret>" --project GymMangamentSystem
dotnet user-secrets set "JWT:ValidIssuer" "<issuer>" --project GymMangamentSystem
dotnet user-secrets set "JWT:ValidAudience" "<audience>" --project GymMangamentSystem
dotnet user-secrets set "MailSettings:Email" "<smtp-email>" --project GymMangamentSystem
dotnet user-secrets set "MailSettings:Password" "<smtp-password>" --project GymMangamentSystem
dotnet user-secrets set "CloudinarySetting:CloudName" "<cloud-name>" --project GymMangamentSystem
dotnet user-secrets set "CloudinarySetting:ApiKey" "<api-key>" --project GymMangamentSystem
dotnet user-secrets set "CloudinarySetting:ApiSecret" "<api-secret>" --project GymMangamentSystem
```

Configure `ClientSettings:AllowedOrigins` and `ClientSettings:EmailConfirmationRedirectUrl` for the frontend environment.

### 3. Apply migrations and run

```powershell
dotnet ef database update --project GymMangamentSystem.Reposatory --startup-project GymMangamentSystem
dotnet run --project GymMangamentSystem
```

Local launch profiles use `https://localhost:7187` and `http://localhost:5159`. Swagger is available at `/swagger` in Development.

## API Modules

| Module | Responsibility |
| --- | --- |
| Account and Auth | Identity lifecycle, login, refresh and revoke flows |
| Attendance and Memberships | Member access and membership records |
| Classes | Class scheduling and management |
| Exercises and Categories | Exercise library organization |
| Workouts and Nutrition | Member plans, meals and meal categories |
| BMI Records | Measurement history and BMI categorization |
| Feedback and Notifications | Member communication workflows |

Collection endpoints accept pagination parameters:

```text
?pageNumber=1&pageSize=20
```

Page size is limited to the range 1-100.

## Automated Tests

```powershell
dotnet test GymMangamentSystem.sln
```

Current suite: **106 test cases**.

- 103 unit test cases covering authentication services, refresh-token rotation, OTP behavior, mapping, models, controllers, and BMI logic.
- 3 SQL Server/LocalDB integration tests covering repository and soft-delete behavior.

Integration tests create isolated temporary databases and remove them after execution.

## Database and Security Notes

- Recent migrations activate soft-delete fields and add frequently used lookup indexes.
- Password rules and lockout behavior are configured through ASP.NET Core Identity.
- The application fails fast when the JWT signing key or required connection string is missing.
- Use reviewed origins, HTTPS, a secrets provider, restricted database credentials, and an external logging target for production.
- Rotate any credential that was previously exposed, even after removing it from Git history.

## Author

**Youssef Ahmed**
[LinkedIn](https://www.linkedin.com/in/youssef-ahmed-eg/) | [GitHub](https://github.com/uosefahmed22) | [Portfolio](https://uosefahmed22.github.io/)
