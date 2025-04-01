# Fase base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ControleLancamento.Api/ControleLancamento.Api.csproj", "src/ControleLancamento.Api/"]
COPY ["src/ControleLancamentos.Application/ControleLancamentos.Application.csproj", "src/ControleLancamentos.Application/"]
COPY ["src/ControleLancamentos.Domain/ControleLancamentos.Domain.csproj", "src/ControleLancamentos.Domain/"]
COPY ["src/ControleLancamentos.IoC/ControleLancamentos.IoC.csproj", "src/ControleLancamentos.IoC/"]
COPY ["src/ControleLancamentos.Messaging/ControleLancamentos.Messaging.csproj", "src/ControleLancamentos.Messaging/"]
COPY ["src/ControleLancamentos.ORM/ControleLancamentos.ORM.csproj", "src/ControleLancamentos.ORM/"]
RUN dotnet restore "./src/ControleLancamento.Api/ControleLancamento.Api.csproj"
COPY . .
WORKDIR "/src/src/ControleLancamento.Api"
RUN dotnet build "./ControleLancamento.Api.csproj" -c Release -o /app/build

# Fase de publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ControleLancamento.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ControleLancamento.Api.dll"]