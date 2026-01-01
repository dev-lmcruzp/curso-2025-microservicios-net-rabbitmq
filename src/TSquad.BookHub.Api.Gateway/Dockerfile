FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
# EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TSquad.BookHub.Api.Gateway/TSquad.BookHub.Api.Gateway.csproj", "src/TSquad.BookHub.Api.Gateway/"]
RUN dotnet restore "src/TSquad.BookHub.Api.Gateway/TSquad.BookHub.Api.Gateway.csproj"
COPY . .
WORKDIR "/src/src/TSquad.BookHub.Api.Gateway"
RUN dotnet build "./TSquad.BookHub.Api.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TSquad.BookHub.Api.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TSquad.BookHub.Api.Gateway.dll"]
