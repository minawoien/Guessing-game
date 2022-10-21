Set-Location .\frontend
npm ci
npm run buildRelease
Set-Location ..
dotnet build
Set-Location Backend
.\dbsetup.ps1
Set-Location ..
Write-Output "Frontend is built, migrations has been run on backend and the database has been created"
Write-Output "Run project from backend folder, the images will be imported on first startup"
