# Gym Management API

ASP.NET Core 8 Web API for gym memberships, classes, attendance, workout and nutrition plans, meals, BMI tracking, feedback, notifications, and Identity-based authentication.

## Architecture

The repository uses a pragmatic three-layer (N-Tier) structure:

- `GymMangamentSystem` — API controllers, middleware, configuration, Swagger, CORS, and dependency injection.
- `GymMangamentSystem.Core` — entities, DTOs, enums, interfaces, validation models, and shared contracts.
- `GymMangamentSystem.Reposatory` — EF Core persistence, business repositories, authentication services, email, mapping, migrations, and interceptors.
- `GymMangamentSystem.UnitTests` — isolated unit tests using xUnit and test doubles.
- `GymMangamentSystem.IntegrationTests` — SQL Server/LocalDB integration tests for EF Core behavior.

This is N-Tier architecture, not a strict Clean Architecture implementation.

## Main Features

- ASP.NET Core Identity with JWT access tokens and rotating refresh tokens.
- Role-based authorization.
- Email confirmation, OTP password reset, and SMTP email delivery.
- Soft delete through an EF Core `SaveChangesInterceptor` and global query filters.
- Explicit application mapper with no AutoMapper runtime or license dependency.
- Paginated read endpoints using `pageNumber` and `pageSize` (default 20, maximum 100).
- No-tracking EF Core list queries and indexes for frequent lookups.
- Swagger/OpenAPI with Bearer authentication support.
- Structured request/application logging through Serilog (console and 14-day rolling files).
- Cloudinary image upload integration.

## Requirements

- .NET 8 SDK
- SQL Server or SQL Server LocalDB
- SMTP account for email features
- Cloudinary account for image features

## Local Configuration

Secrets must not be committed to `appsettings*.json`. Initialize local User Secrets from the API project:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<connection-string>" --project GymMangamentSystem
dotnet user-secrets set "JWT:Key" "<at-least-32-byte-secret>" --project GymMangamentSystem
dotnet user-secrets set "MailSettings:Email" "<smtp-email>" --project GymMangamentSystem
dotnet user-secrets set "MailSettings:Password" "<smtp-password>" --project GymMangamentSystem
dotnet user-secrets set "CloudinarySetting:CloudName" "<cloud-name>" --project GymMangamentSystem
dotnet user-secrets set "CloudinarySetting:ApiKey" "<api-key>" --project GymMangamentSystem
dotnet user-secrets set "CloudinarySetting:ApiSecret" "<api-secret>" --project GymMangamentSystem
```

Safe non-secret values and placeholders are documented in `GymMangamentSystem/appsettings.Example.json`. Configure `ClientSettings:AllowedOrigins` with the real frontend origins and `ClientSettings:EmailConfirmationRedirectUrl` with the frontend confirmation page.

## Run the Project

```powershell
dotnet restore
dotnet ef database update --project GymMangamentSystem.Reposatory --startup-project GymMangamentSystem
dotnet run --project GymMangamentSystem
```

Swagger is available from the URL printed by the application in Development.

## Run the Tests

```powershell
dotnet test GymMangamentSystem.sln
```

Current automated suite: 103 unit test cases and 3 integration tests (106 total). Integration tests require SQL Server LocalDB and create isolated temporary databases that are deleted after each test.

## Pagination

Collection endpoints accept query parameters:

```text
?pageNumber=1&pageSize=20
```

Invalid page sizes are clamped to 1–100 and page numbers below 1 are treated as page 1.

## Database Migrations

The latest migrations activate production soft delete fields and add lookup/performance indexes. Review generated SQL before applying migrations to production:

```powershell
dotnet ef migrations script --idempotent --project GymMangamentSystem.Reposatory --startup-project GymMangamentSystem
```

## Security Notes

- Passwords require at least 8 characters with upper/lowercase letters and a digit.
- Accounts are locked for 15 minutes after 5 failed attempts.
- CORS uses configured origins rather than `AllowAnyOrigin`.
- JWT configuration fails fast when the signing key is missing or too short.
- Previously exposed credentials must still be rotated at their providers, even after Git history is cleaned.
