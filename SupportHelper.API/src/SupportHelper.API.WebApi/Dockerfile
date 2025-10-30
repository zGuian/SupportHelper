# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SupportHelper.API.WebApi/SupportHelper.API.WebApi.csproj", "src/SupportHelper.API.WebApi/"]
COPY ["src/SupportHelper.API.Infra.CrossCutting/SupportHelper.API.Infra.CrossCutting.csproj", "src/SupportHelper.API.Infra.CrossCutting/"]
COPY ["src/SupportHelper.API.Domain/SupportHelper.API.Domain.csproj", "src/SupportHelper.API.Domain/"]
COPY ["src/SupportHelper.API.Infra.Data/SupportHelper.API.Infra.Data.csproj", "src/SupportHelper.API.Infra.Data/"]
COPY ["src/SupportHelper.API.Infra.SignalR/SupportHelper.API.Infra.SignalR.csproj", "src/SupportHelper.API.Infra.SignalR/"]
RUN dotnet restore "./src/SupportHelper.API.WebApi/SupportHelper.API.WebApi.csproj"
COPY . .
WORKDIR "/src/src/SupportHelper.API.WebApi"
RUN dotnet build "./SupportHelper.API.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SupportHelper.API.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SupportHelper.API.WebApi.dll"]