FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TechChallenge01.Infra.Consumer/TechChallenge01.Infra.Consumer.csproj", "src/TechChallenge01.Infra.Consumer/"]
COPY ["src/TechChallenge01.Infra.Consumer.IoC/TechChallenge01.Infra.Consumer.IoC.csproj", "src/TechChallenge01.Infra.Consumer.IoC/"]
RUN dotnet restore "./src/TechChallenge01.Infra.Consumer/TechChallenge01.Infra.Consumer.csproj"
COPY . .
WORKDIR "/src/src/TechChallenge01.Infra.Consumer"
RUN dotnet build "./TechChallenge01.Infra.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallenge01.Infra.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallenge01.Infra.Consumer.dll"]