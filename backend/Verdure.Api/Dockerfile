#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Verdure.Api/Verdure.Api.csproj", "Verdure.Api/"]
COPY ["Verdure.Infrastructure/Verdure.Infrastructure.csproj", "Verdure.Infrastructure/"]
COPY ["Verdure.ApplicationCore/Verdure.ApplicationCore.csproj", "Verdure.ApplicationCore/"]
RUN dotnet restore "Verdure.Api/Verdure.Api.csproj"
COPY . .
WORKDIR "/src/Verdure.Api"
RUN dotnet build "Verdure.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Verdure.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Verdure.Api.dll"]