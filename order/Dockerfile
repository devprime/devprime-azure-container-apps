# Step 1
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Step 2
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN echo "Starting copy..."
WORKDIR /
COPY [".", "MyApp/"]
RUN dotnet restore "MyApp/order.sln"
#COPY . .
WORKDIR "/MyApp"
RUN echo "Build..."
RUN dotnet build "order.sln" -c Release -o /app/build --no-restore

# Step 3
RUN echo "Publish..."
FROM build AS publish
RUN dotnet publish "order.sln" -c Release -o /app/publish --no-restore

# Step 4
FROM base AS final
WORKDIR /app
RUN echo "Copy..."
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.dll"]