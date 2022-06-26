#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Content/Verdure.Content.Api/Verdure.Content.Api.csproj", "src/Services/Content/Verdure.Content.Api/"]
COPY ["src/Services/Content/Verdure.Infrastructure/Verdure.Infrastructure.csproj", "src/Services/Content/Verdure.Infrastructure/"]
COPY ["src/Services/Content/Verdure.Core/Verdure.Core.csproj", "src/Services/Content/Verdure.Core/"]
COPY ["src/Common/Verdure.Common/Verdure.Common.csproj", "src/Common/Verdure.Common/"]
COPY ["src/Services/Content/Verdure.Data.Mongo/Verdure.Data.Mongo.csproj", "src/Services/Content/Verdure.Data.Mongo/"]
RUN dotnet restore "src/Services/Content/Verdure.Content.Api/Verdure.Content.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Content/Verdure.Content.Api"
RUN dotnet build "Verdure.Content.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Verdure.Content.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Verdure.Content.Api.dll"]