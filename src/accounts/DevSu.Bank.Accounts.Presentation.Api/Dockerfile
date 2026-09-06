# Imagen base de ejecucion
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Compilacion
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DevSu.Bank.Accounts.Presentation.Api/DevSu.Bank.Accounts.Presentation.Api.csproj", "DevSu.Bank.Accounts.Presentation.Api/"]
COPY ["DevSu.Bank.Accounts.Infrastructure/DevSu.Bank.Accounts.Infrastructure.csproj", "DevSu.Bank.Accounts.Infrastructure/"]
COPY ["DevSu.Bank.Accounts.Application/DevSu.Bank.Accounts.Application.csproj", "DevSu.Bank.Accounts.Application/"]
COPY ["DevSu.Bank.Accounts.Domain/DevSu.Bank.Accounts.Domain.csproj", "DevSu.Bank.Accounts.Domain/"]
RUN dotnet restore "./DevSu.Bank.Accounts.Presentation.Api/DevSu.Bank.Accounts.Presentation.Api.csproj"
COPY . .
WORKDIR "/src/DevSu.Bank.Accounts.Presentation.Api"
RUN dotnet build "./DevSu.Bank.Accounts.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicacion
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DevSu.Bank.Accounts.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevSu.Bank.Accounts.Presentation.Api.dll"]
