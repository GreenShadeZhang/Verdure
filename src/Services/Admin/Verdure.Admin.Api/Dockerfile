#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Admin/Verdure.Admin.Api/Verdure.Admin.Api.csproj", "src/Services/Admin/Verdure.Admin.Api/"]
RUN dotnet restore "src/Services/Admin/Verdure.Admin.Api/Verdure.Admin.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Admin/Verdure.Admin.Api"
RUN dotnet build "Verdure.Admin.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Verdure.Admin.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Verdure.Admin.Api.dll"]