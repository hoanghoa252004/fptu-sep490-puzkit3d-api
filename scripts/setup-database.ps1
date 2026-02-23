# ?? QUICK SETUP DATABASE - Ch? 30 giây!

Write-Host "?? SETUP DATABASE CHO LOGIN API" -ForegroundColor Green
Write-Host "================================`n" -ForegroundColor Green

# Thông tin database
$dbName = "puzkit3d_test"
$dbUser = "postgres"
$dbPassword = "12345"

Write-Host "?? Database: $dbName" -ForegroundColor Cyan
Write-Host "?? User: $dbUser" -ForegroundColor Cyan
Write-Host "?? Password: ****`n" -ForegroundColor Cyan

# Ki?m tra n?u EF Core tools ch?a có
Write-Host "?? Checking EF Core Tools..." -ForegroundColor Yellow
$efTool = dotnet tool list --global | Select-String "dotnet-ef"
if (-not $efTool) {
    Write-Host "? Installing EF Core Tools..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    Write-Host "? EF Core Tools installed!" -ForegroundColor Green
}

# Set password environment
$env:PGPASSWORD = $dbPassword

# B??c 1: T?o database (n?u ch?a có)
Write-Host "`n?? Step 1: Creating database..." -ForegroundColor Cyan
try {
    psql -U $dbUser -c "CREATE DATABASE $dbName;" 2>&1 | Out-Null
    Write-Host "? Database '$dbName' created!" -ForegroundColor Green
} catch {
    Write-Host "?? Database already exists (OK)" -ForegroundColor Yellow
}

# B??c 2: Apply migration
Write-Host "`n?? Step 2: Applying migrations..." -ForegroundColor Cyan
$connectionString = "Host=localhost;Port=5432;Database=$dbName;Username=$dbUser;Password=$dbPassword"

dotnet ef database update `
    --context IdentityDbContext `
    --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure `
    --startup-project src\WebApi\PuzKit3D.WebApi `
    --connection $connectionString

if ($LASTEXITCODE -eq 0) {
    Write-Host "? Migrations applied successfully!" -ForegroundColor Green
} else {
    Write-Host "? Migration failed! Check errors above." -ForegroundColor Red
    exit 1
}

# B??c 3: Ki?m tra tables
Write-Host "`n?? Step 3: Verifying tables..." -ForegroundColor Cyan
$tables = psql -U $dbUser -d $dbName -t -c "\dt identity.*" 2>&1
if ($tables) {
    Write-Host "? Identity tables created successfully!" -ForegroundColor Green
    Write-Host $tables
} else {
    Write-Host "?? Could not verify tables" -ForegroundColor Yellow
}

# Hi?n th? roles ?ã seed
Write-Host "`n?? Seeded Roles:" -ForegroundColor Cyan
psql -U $dbUser -d $dbName -c "SELECT \"Name\", \"Description\" FROM identity.identity_role;"

Write-Host "`n" 
Write-Host "?? DATABASE SETUP HOÀN T?T!" -ForegroundColor Green
Write-Host "================================" -ForegroundColor Green
Write-Host "`n?? Ti?p theo:" -ForegroundColor Yellow
Write-Host "   1. Ch?y app: .\scripts\run-api.ps1" -ForegroundColor White
Write-Host "   2. M? Swagger: http://localhost:5000/swagger" -ForegroundColor White
Write-Host "   3. Test Login v?i: admin@puzkit3d.com / Admin123!@#`n" -ForegroundColor White
