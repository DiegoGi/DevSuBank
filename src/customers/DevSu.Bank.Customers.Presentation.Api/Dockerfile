# Imagen base de ejecucion
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Compilacion
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DevSu.Bank.Customers.Presentation.Api/DevSu.Bank.Customers.Presentation.Api.csproj", "DevSu.Bank.Customers.Presentation.Api/"]
COPY ["DevSu.Bank.Customers.Infrastructure/DevSu.Bank.Customers.Infrastructure.csproj", "DevSu.Bank.Customers.Infrastructure/"]
COPY ["DevSu.Bank.Customers.Application/DevSu.Bank.Customers.Application.csproj", "DevSu.Bank.Customers.Application/"]
COPY ["DevSu.Bank.Customers.Domain/DevSu.Bank.Customers.Domain.csproj", "DevSu.Bank.Customers.Domain/"]
RUN dotnet restore "./DevSu.Bank.Customers.Presentation.Api/DevSu.Bank.Customers.Presentation.Api.csproj"
COPY . .
WORKDIR "/src/DevSu.Bank.Customers.Presentation.Api"
RUN dotnet build "./DevSu.Bank.Customers.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicacion
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DevSu.Bank.Customers.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevSu.Bank.Customers.Presentation.Api.dll"]
