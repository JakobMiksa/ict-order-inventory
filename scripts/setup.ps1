$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $root 'docker\docker-compose.yml'
$sqlFile = Join-Path $root 'database\00-create-database.sql'
$apiProject = Join-Path $root 'src\ICT.Api'
$container = 'mssqlServer'

function Update-Path {
    $machine = [Environment]::GetEnvironmentVariable('Path', 'Machine')
    $user = [Environment]::GetEnvironmentVariable('Path', 'User')
    $env:Path = "$machine;$user"
}

function Test-Command($name) {
    return [bool](Get-Command $name -ErrorAction SilentlyContinue)
}

function Install-Package($id) {
    if (-not (Test-Command winget)) {
        throw 'winget findes ikke. Opdater App Installer fra Microsoft Store og kør scriptet igen.'
    }
    winget install --id $id -e --source winget
    Update-Path
}

function Test-DockerReady {
    if (-not (Test-Command docker)) { return $false }
    docker info *> $null
    return ($LASTEXITCODE -eq 0)
}

Update-Path

$hasSdk = $false
if (Test-Command dotnet) {
    $hasSdk = [bool]((dotnet --list-sdks) -match '^10\.')
}
if (-not $hasSdk) {
    Write-Host 'Installerer .NET SDK 10'
    Install-Package 'Microsoft.DotNet.SDK.10'
    if (-not (Test-Command dotnet)) {
        throw '.NET SDK blev installeret, men dotnet findes ikke endnu. Luk terminalen, åbn en ny og kør scriptet igen.'
    }
}
Write-Host ".NET SDK: $(dotnet --version)"

if (-not (Test-Command docker)) {
    Write-Host 'Installerer Docker Desktop (accepter vilkårene og klik Ja i UAC)'
    Install-Package 'Docker.DockerDesktop'
    Write-Host 'Docker Desktop er installeret. Genstart PC, start Docker Desktop, vent til den kører, og kør scriptet igen.'
    exit 0
}

if (-not (Test-DockerReady)) {
    $desktop = @(
        (Join-Path $env:ProgramFiles 'Docker\Docker\Docker Desktop.exe'),
        (Join-Path $env:LOCALAPPDATA 'Programs\DockerDesktop\Docker Desktop.exe')
    ) | Where-Object { Test-Path $_ } | Select-Object -First 1
    if ($desktop) {
        Write-Host 'Starter Docker Desktop'
        Start-Process $desktop
    }
    Write-Host 'Venter på Docker (op til 3 minutter)'
    $deadline = (Get-Date).AddMinutes(3)
    while (-not (Test-DockerReady) -and (Get-Date) -lt $deadline) {
        Start-Sleep -Seconds 5
    }
    if (-not (Test-DockerReady)) {
        throw 'Docker kører ikke. Åbn Docker Desktop, accepter vilkårene, vent til "Engine running", og kør scriptet igen.'
    }
}

$saPassword = $env:SA_PASSWORD
if (-not $saPassword) {
    $match = [regex]::Match((Get-Content $composeFile -Raw), 'SA_PASSWORD:\s*"\$\{SA_PASSWORD:-([^}]*)\}"')
    if (-not $match.Success) {
        throw 'Kunne ikke læse SA_PASSWORD fra docker-compose.yml.'
    }
    $saPassword = $match.Groups[1].Value
}

$existing = docker ps -a --filter "name=^/$container$" --format '{{.Names}}'
if ($existing) {
    Write-Host "Genbruger eksisterende container $container"
    docker start $container | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "docker start $container fejlede." }
} else {
    Write-Host 'Starter databasecontaineren'
    docker compose -f $composeFile up -d
    if ($LASTEXITCODE -ne 0) { throw 'docker compose up fejlede.' }
}

$sqlcmd = '/opt/mssql-tools18/bin/sqlcmd'
Write-Host 'Venter på SQL Server (op til 3 minutter)'
$ready = $false
$deadline = (Get-Date).AddMinutes(3)
while (-not $ready -and (Get-Date) -lt $deadline) {
    docker exec -e "SQLCMDPASSWORD=$saPassword" $container $sqlcmd -S localhost -U sa -C -Q 'SELECT 1' *> $null
    if ($LASTEXITCODE -eq 0) {
        $ready = $true
    } else {
        Start-Sleep -Seconds 3
    }
}
if (-not $ready) { throw 'SQL Server blev ikke klar i tide. Se: docker logs mssqlServer' }

Write-Host 'Opretter ICTOrderDb hvis den mangler'
docker cp $sqlFile "${container}:/tmp/00-create-database.sql"
docker exec -e "SQLCMDPASSWORD=$saPassword" $container $sqlcmd -S localhost -U sa -C -b -i /tmp/00-create-database.sql
if ($LASTEXITCODE -ne 0) { throw 'Kørsel af 00-create-database.sql fejlede.' }

Write-Host 'Sætter user secret i ICT.Api'
$connectionString = "Server=localhost,1433;Database=ICTOrderDb;User Id=sa;Password=$saPassword;TrustServerCertificate=True"
Push-Location $apiProject
try {
    dotnet user-secrets set 'ConnectionStrings:ICTOrderDb' $connectionString | Out-Null
    if ($LASTEXITCODE -ne 0) { throw 'dotnet user-secrets fejlede.' }
} finally {
    Pop-Location
}

Write-Host 'Klar. Byg og test med: dotnet build ICT.slnx ; dotnet test ICT.slnx'
