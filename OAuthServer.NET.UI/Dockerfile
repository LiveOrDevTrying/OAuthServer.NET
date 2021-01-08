#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM node:latest AS node_base

FROM mcr.microsoft.com/dotnet/aspnet:5.0.1-buster-slim-arm32v7 AS base
COPY --from=node_base . .
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
COPY --from=node_base . .
WORKDIR /src
COPY ["OAuthServer.NET.UI/OAuthServer.NET.UI.csproj", "OAuthServer.NET.UI/"]
RUN dotnet restore "OAuthServer.NET.UI/OAuthServer.NET.UI.csproj" -r linux-arm
COPY . .
WORKDIR "/src/OAuthServer.NET.UI"
RUN dotnet build "OAuthServer.NET.UI.csproj" -c Release -o /app/build -r linux-arm

FROM build AS publish
RUN dotnet publish "OAuthServer.NET.UI.csproj" -c Release -o /app/publish -r linux-arm

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OAuthServer.NET.UI.dll"]