#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["King.Tickets.API/King.Tickets.API.csproj", "King.Tickets.API/"]
COPY ["King.Tickets.Infrastructure/King.Tickets.Infrastructure.csproj", "King.Tickets.Infrastructure/"]
COPY ["King.Tickets.Domain/King.Tickets.Domain.csproj", "King.Tickets.Domain/"]
RUN dotnet restore "./King.Tickets.API/King.Tickets.API.csproj"
COPY . .
WORKDIR "/src/King.Tickets.API"
RUN dotnet build "./King.Tickets.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./King.Tickets.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "King.Tickets.API.dll"]