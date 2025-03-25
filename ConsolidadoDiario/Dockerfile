# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
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

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ConsolidadoDiario.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsolidadoDiario.Api.dll"]