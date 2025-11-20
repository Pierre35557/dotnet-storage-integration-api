# ------ Build Image ------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /web

COPY . .

# TEMP: list files so we can see the actual structure during docker build
RUN ls -R .

RUN dotnet restore "./src/StorageIntegrationApi/StorageIntegrationApi.Api.csproj"
RUN dotnet publish "./src/StorageIntegrationApi/StorageIntegrationApi.Api.csproj" \
    -c Release \
    -o /web/publish \
    --no-restore

# ------ Runtime ------
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS production
WORKDIR /web

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /web/publish .

CMD ["dotnet", "StorageIntegrationApi.Api.dll"]