#!/bin/bash
echo "Start restoring..."
dotnet restore
echo "Start building..."
dotnet build -c release --no-restore
echo "Start running..."
dotnet run --project ./src/App/App.csproj -c release