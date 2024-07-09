#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TechChallenge01.Api/TechChallenge01.Api.csproj", "src/TechChallenge01.Api/"]
COPY ["src/TechChallenge01.Infra.IoC/TechChallenge01.Infra.IoC.csproj", "src/TechChallenge01.Infra.IoC/"]
COPY ["src/TechChallenge01.Infra.Data/TechChallenge01.Infra.Data.csproj", "src/TechChallenge01.Infra.Data/"]
COPY ["src/TechChallenge01.Application/TechChallenge01.Application.csproj", "src/TechChallenge01.Application/"]
COPY ["src/TechChallenge01.Domain/TechChallenge01.Domain.csproj", "src/TechChallenge01.Domain/"]
RUN dotnet restore "./src/TechChallenge01.Api/./TechChallenge01.Api.csproj"
COPY . .
WORKDIR "/src/src/TechChallenge01.Api"
RUN dotnet build "./TechChallenge01.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallenge01.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallenge01.Api.dll"]