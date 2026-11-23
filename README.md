# ICT Order and Inventory

Semesterprojekt på 3. semester, Gruppe 1 (Christian og Jakob).

Et bestillingssystem til ICT. Medlemmer bestiller varer på en hjemmeside, og admin styrer varer og lager i et PC-program.

## Kør projektet

1. Start databasen: `docker compose up -d` i mappen `docker`
2. Kør scripts i mappen `database` i SSMS i nummerorden (00, 01)
3. Sæt connection string i `src/ICT.Api` med `dotnet user-secrets`
4. `dotnet build` og `dotnet test`

## Arbejdsgang

Backloggen ligger som issues, og vi bruger en branch pr. user story, fx `feature/1-se-varer`. Vi merger til main med pull requests.
