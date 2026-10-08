# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first so restore is cached unless dependencies change
COPY src/InventoryApi.Domain/InventoryApi.Domain.csproj src/InventoryApi.Domain/
COPY src/InventoryApi.Application/InventoryApi.Application.csproj src/InventoryApi.Application/
COPY src/InventoryApi.Infrastructure/InventoryApi.Infrastructure.csproj src/InventoryApi.Infrastructure/
COPY src/InventoryApi.Api/InventoryApi.Api.csproj src/InventoryApi.Api/
RUN dotnet restore src/InventoryApi.Api/InventoryApi.Api.csproj

COPY src/ src/
RUN dotnet publish src/InventoryApi.Api/InventoryApi.Api.csproj -c Release -o /app/publish --no-restore

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# SQLite needs a writable folder; then drop to the image's non-root user
RUN mkdir -p /app/data && chown -R $APP_UID /app/data
USER $APP_UID

ENV ConnectionStrings__Default="Data Source=/app/data/inventory.db"
EXPOSE 8080
ENTRYPOINT ["dotnet", "InventoryApi.Api.dll"]