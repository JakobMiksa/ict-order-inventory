# ICT Order and Inventory Management System

Semesterprojekt, Gruppe 1 (Christian, Jakob). Datamatiker 3. semester, UCN 2026.

Ordre- og lagersystem for ICTrust, hvor medlemmer bestiller via web, og administratoren styrer varer og lager via en PC-klient — uden oversalg ved samtidige bestillinger.

## Struktur

| Projekt | Rolle |
|---|---|
| `src/ICT.Api` | REST-service (ASP.NET Core Web API) |
| `src/ICT.BusinessLogic` | Forretningslogik |
| `src/ICT.DataAccess` | Dataadgang (ADO.NET mod MS SQL Server) |
| `src/ICT.Model` | Domæneklasser, deles af alle lag |
| `src/ICT.Web` | Web-klient til medlemmer (ASP.NET Core MVC) |
| `src/ICT.Desktop` | PC-klient til administrator (Windows Forms) |
| `tests/ICT.Tests` | Unit tests (xUnit) |
| `database/` | SQL-scripts, kørt i nummerorden |
| `docker/` | MS SQL Server i Docker |

Klienterne taler kun med API'et; kun API'et taler med databasen.

## Kom i gang

1. Start Docker Desktop og databasen:
   ```
   cd docker
   docker compose up -d
   ```
2. Opret databasen: åbn `database/00-create-database.sql` i SSMS (server `localhost,1433`, login `sa`) og kør den.
3. Gem connection string som user secret (holdes uden for git):
   ```
   cd src/ICT.Api
   dotnet user-secrets set "ConnectionStrings:ICTOrderDb" "Server=localhost,1433;Database=ICTOrderDb;User Id=sa;Password=!Password;TrustServerCertificate=True"
   ```
4. Byg og test:
   ```
   dotnet build ICT.slnx
   dotnet test ICT.slnx
   ```

## Arbejdsgang

- Én branch pr. user story, fx `feature/US3-bestil`.
- Merge til `main` kun via pull request, som en anden i gruppen har godkendt.
- GitHub Actions bygger og kører tests ved hvert push og pull request.
