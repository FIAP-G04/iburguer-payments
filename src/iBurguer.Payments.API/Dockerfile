FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["iBurguer.Payments.API/iBurguer.Payments.API.csproj", "iBurguer.Payments.API/"]
COPY ["iBurguer.Payments.Core/iBurguer.Payments.Core.csproj", "iBurguer.Payments.Core/"]
COPY ["iBurguer.Payments.Infrastructure/iBurguer.Payments.Infrastructure.csproj", "iBurguer.Payments.Infrastructure/"]
RUN dotnet restore "iBurguer.Payments.API/iBurguer.Payments.API.csproj"
COPY . .
WORKDIR "/src/iBurguer.Payments.API"
RUN dotnet build "iBurguer.Payments.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "iBurguer.Payments.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iBurguer.Payments.API.dll"]
