#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Reviews/Verdure.Reviews.Api/Verdure.Reviews.Api.csproj", "src/Services/Reviews/Verdure.Reviews.Api/"]
RUN dotnet restore "src/Services/Reviews/Verdure.Reviews.Api/Verdure.Reviews.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Reviews/Verdure.Reviews.Api"
RUN dotnet build "Verdure.Reviews.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Verdure.Reviews.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Verdure.Reviews.Api.dll"]