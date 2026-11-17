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

## Kom i gang på en ny Windows-PC

1. Klon repoet og åbn PowerShell i repoets rod.
2. Kør `powershell -ExecutionPolicy Bypass -File scripts/setup.ps1`.
3. Første gang installerer scriptet .NET SDK 10 og Docker Desktop med winget. Klik Ja i UAC, og accepter Docker Desktops vilkår selv. Genstart PC'en hvis Docker eller WSL2 beder om det, start Docker Desktop, vent til den viser "Engine running", og kør scriptet igen.
4. Scriptet starter databasen, opretter `ICTOrderDb` og sætter user secret. Det kan køres igen uden skade.
5. Byg og test: `dotnet build ICT.slnx` og `dotnet test ICT.slnx`.

## Kom i gang manuelt

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

- Backloggen er GitHub Issues (#1-#12), og sprintene styres på tavlen: https://github.com/users/JakobMiksa/projects/3
- Én branch pr. user story med issue-nummeret i navnet, fx `feature/1-se-varer`.
- Commit-beskeder nævner issue-nummeret, fx `Vareliste på hjemmesiden (#1)`.
- Merge til `main` kun via pull request, som den anden i gruppen har godkendt. Pull requesten skriver `Closes #1`, så issuet lukkes og flyttes til Done.
- Definition of Done står i pull request-skabelonen.
- GitHub Actions bygger og kører tests ved hvert push og pull request.
