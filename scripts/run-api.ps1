# Script ??n gi?n ?? ch?y API
Write-Host "?? Starting PuzKit3D API..." -ForegroundColor Green
Write-Host "?? Database: puzkit3d_test" -ForegroundColor Cyan
Write-Host "?? Swagger UI s? m? t?i: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host "`n? ?ang kh?i ??ng (app s? t? ??ng seed test users)...`n" -ForegroundColor Yellow

Set-Location src\WebApi\PuzKit3D.WebApi
dotnet run

Write-Host "`n? API ?ã d?ng." -ForegroundColor Green
