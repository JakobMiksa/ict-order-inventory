# ICT Order and Inventory

Semesterprojekt på 3. semester, Gruppe 1 (Christian og Jakob).

Et bestillingssystem til ICT. Medlemmer bestiller varer på en hjemmeside, og admin styrer varer og lager i et PC-program.

## Kør projektet

1. Start databasen: `docker compose up -d` i mappen `docker`
2. Kør `database/00-create-database.sql` i SSMS
3. Sæt connection string i `src/ICT.Api` med `dotnet user-secrets`
4. `dotnet build` og `dotnet test`
