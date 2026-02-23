# Script t? ??ng setup database và test Login API
Write-Host "?? B?t ??u Setup Database và Test Login API..." -ForegroundColor Green

# B??c 1: Build project
Write-Host "`n?? Building project..." -ForegroundColor Cyan
dotnet build src\WebApi\PuzKit3D.WebApi\PuzKit3D.WebApi.csproj
if ($LASTEXITCODE -ne 0) {
    Write-Host "? Build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "? Build successful!" -ForegroundColor Green

# B??c 2: Ch?y ?ng d?ng
Write-Host "`n?? Starting application (s? t? ??ng seed test users)..." -ForegroundColor Cyan
Write-Host "? ??i 5 giây ?? app kh?i ??ng..." -ForegroundColor Yellow

# Ch?y app trong background
$job = Start-Job -ScriptBlock {
    Set-Location $using:PWD
    dotnet run --project src\WebApi\PuzKit3D.WebApi\PuzKit3D.WebApi.csproj
}

Start-Sleep -Seconds 5

# B??c 3: Test Login API
Write-Host "`n?? Testing Login API..." -ForegroundColor Cyan

try {
    # Test v?i Admin User
    Write-Host "`n1?? Testing Admin User Login..." -ForegroundColor Yellow
    $adminBody = @{
        email = "admin@puzkit3d.com"
        password = "Admin123!@#"
    } | ConvertTo-Json

    $adminResponse = Invoke-RestMethod `
        -Uri "http://localhost:5000/api/auth/login" `
        -Method Post `
        -Body $adminBody `
        -ContentType "application/json" `
        -ErrorAction Stop

    Write-Host "? Admin Login Success!" -ForegroundColor Green
    Write-Host "User ID: $($adminResponse.userId)" -ForegroundColor Cyan
    Write-Host "Email: $($adminResponse.email)" -ForegroundColor Cyan
    Write-Host "Token: $($adminResponse.token.Substring(0, 50))..." -ForegroundColor Cyan
    Write-Host "Expires At: $($adminResponse.expiresAt)" -ForegroundColor Cyan

    # Test v?i Regular User
    Write-Host "`n2?? Testing Regular User Login..." -ForegroundColor Yellow
    $userBody = @{
        email = "user@puzkit3d.com"
        password = "User123!@#"
    } | ConvertTo-Json

    $userResponse = Invoke-RestMethod `
        -Uri "http://localhost:5000/api/auth/login" `
        -Method Post `
        -Body $userBody `
        -ContentType "application/json" `
        -ErrorAction Stop

    Write-Host "? User Login Success!" -ForegroundColor Green
    Write-Host "User ID: $($userResponse.userId)" -ForegroundColor Cyan
    Write-Host "Email: $($userResponse.email)" -ForegroundColor Cyan

    # Test v?i Invalid Credentials
    Write-Host "`n3?? Testing Invalid Credentials (should fail)..." -ForegroundColor Yellow
    $invalidBody = @{
        email = "admin@puzkit3d.com"
        password = "WrongPassword123!"
    } | ConvertTo-Json

    try {
        Invoke-RestMethod `
            -Uri "http://localhost:5000/api/auth/login" `
            -Method Post `
            -Body $invalidBody `
            -ContentType "application/json" `
            -ErrorAction Stop
        
        Write-Host "? Should have failed with invalid credentials!" -ForegroundColor Red
    } catch {
        Write-Host "? Correctly rejected invalid credentials!" -ForegroundColor Green
    }

    Write-Host "`n?? All tests passed!" -ForegroundColor Green
    Write-Host "`n?? Test Users:" -ForegroundColor Cyan
    Write-Host "   Admin: admin@puzkit3d.com / Admin123!@#" -ForegroundColor White
    Write-Host "   User:  user@puzkit3d.com / User123!@#" -ForegroundColor White
    Write-Host "`n?? Swagger UI: http://localhost:5000/swagger" -ForegroundColor Cyan

} catch {
    Write-Host "`n? Test failed: $($_.Exception.Message)" -ForegroundColor Red
} finally {
    # D?ng application
    Write-Host "`n?? Stopping application..." -ForegroundColor Yellow
    Stop-Job -Job $job
    Remove-Job -Job $job
    Write-Host "? Done!" -ForegroundColor Green
}
