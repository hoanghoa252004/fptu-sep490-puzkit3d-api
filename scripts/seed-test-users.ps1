# Script ?? seed test users tr?c ti?p vào database
Write-Host "?? Seeding test users vào database..." -ForegroundColor Green

$dbName = "puzkit3d_test"
$dbUser = "postgres"
$dbPassword = "12345"
$env:PGPASSWORD = $dbPassword

# Ki?m tra database có t?n t?i không
Write-Host "`n?? Checking database..." -ForegroundColor Cyan
$dbExists = psql -U $dbUser -lqt | Select-String -Pattern $dbName
if (-not $dbExists) {
    Write-Host "? Database '$dbName' không t?n t?i! Ch?y .\scripts\setup-database.ps1 tr??c." -ForegroundColor Red
    exit 1
}
Write-Host "? Database exists!" -ForegroundColor Green

# Ki?m tra roles
Write-Host "`n?? Checking roles..." -ForegroundColor Cyan
$rolesCount = psql -U $dbUser -d $dbName -t -A -c "SELECT COUNT(*) FROM identity.identity_role;"
Write-Host "? Found $rolesCount roles" -ForegroundColor Green

# Build và ch?y app m?t l?n ?? seed
Write-Host "`n??? Building application..." -ForegroundColor Cyan
dotnet build src\WebApi\PuzKit3D.WebApi\PuzKit3D.WebApi.csproj --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "? Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`n?? Starting application to seed data..." -ForegroundColor Cyan
Write-Host "? ??i 10 giây ?? app seed users..." -ForegroundColor Yellow

# Ch?y app trong background
$job = Start-Job -ScriptBlock {
    param($location)
    Set-Location $location
    dotnet run --project src\WebApi\PuzKit3D.WebApi\PuzKit3D.WebApi.csproj --no-build
} -ArgumentList $PWD

# ??i app kh?i ??ng và seed
Start-Sleep -Seconds 10

# D?ng app
Stop-Job -Job $job -ErrorAction SilentlyContinue
Remove-Job -Job $job -Force -ErrorAction SilentlyContinue

# Ki?m tra users ?ã ???c t?o
Write-Host "`n?? Checking seeded users..." -ForegroundColor Cyan
$usersCount = psql -U $dbUser -d $dbName -t -A -c "SELECT COUNT(*) FROM identity.identity_user;" 2>$null

if ($usersCount -and $usersCount -gt 0) {
    Write-Host "? Found $usersCount user(s) in database!" -ForegroundColor Green
    
    Write-Host "`n?? Users in database:" -ForegroundColor Cyan
    psql -U $dbUser -d $dbName -c "SELECT \"Email\", \"FirstName\", \"LastName\" FROM identity.identity_user;"
    
    Write-Host "`n?? Seed completed successfully!" -ForegroundColor Green
    Write-Host "`n?? Test Users:" -ForegroundColor Yellow
    Write-Host "   Admin: admin@puzkit3d.com / Admin123!@#" -ForegroundColor White
    Write-Host "   User:  user@puzkit3d.com / User123!@#" -ForegroundColor White
} else {
    Write-Host "? No users found! Seed might have failed." -ForegroundColor Red
    Write-Host "?? Try running the app manually and check logs:" -ForegroundColor Yellow
    Write-Host "   cd src\WebApi\PuzKit3D.WebApi" -ForegroundColor White
    Write-Host "   dotnet run" -ForegroundColor White
}
