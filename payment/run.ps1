Write-Host "Start restoring..."
dotnet restore
Write-Host "Start building..."
dotnet build -c release --no-restore
Write-Host "Start running..."
dotnet run --project .\src\App\App.csproj -c release --no-launch-profile
