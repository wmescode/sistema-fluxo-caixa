# Base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ConsolidadoDiario.Api/ConsolidadoDiario.Api.csproj", "src/ConsolidadoDiario.Api/"]
COPY ["src/ConsolidadoDiario.IoC/ConsolidadoDiario.IoC.csproj", "src/ConsolidadoDiario.IoC/"]
COPY ["src/ConsolidadoDiario.Application/ConsolidadoDiario.Application.csproj", "src/ConsolidadoDiario.Application/"]
COPY ["src/ConsolidadoDiario.Domain/ConsolidadoDiario.Domain.csproj", "src/ConsolidadoDiario.Domain/"]
COPY ["src/ConsolidadoDiario.ORM/ConsolidadoDiario.ORM.csproj", "src/ConsolidadoDiario.ORM/"]
COPY ["src/ConsolidadoDiario.Messaging/ConsolidadoDiario.Messaging.csproj", "src/ConsolidadoDiario.Messaging/"]
RUN dotnet restore "./src/ConsolidadoDiario.Api/ConsolidadoDiario.Api.csproj"
COPY . .
WORKDIR "/src/src/ConsolidadoDiario.Api"
RUN dotnet build "./ConsolidadoDiario.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase de publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ConsolidadoDiario.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsolidadoDiario.Api.dll"]