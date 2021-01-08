#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["OAuthServer.NET.WebApp/OAuthServer.NET.WebApp.csproj", "OAuthServer.NET.WebApp/"]
RUN dotnet restore "OAuthServer.NET.WebApp/OAuthServer.NET.WebApp.csproj"
COPY . .
WORKDIR "/src/OAuthServer.NET.WebApp"
RUN dotnet build "OAuthServer.NET.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OAuthServer.NET.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OAuthServer.NET.WebApp.dll"]
