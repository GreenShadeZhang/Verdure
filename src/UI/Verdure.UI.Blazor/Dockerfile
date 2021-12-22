#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UI/Verdure.UI.Blazor/Verdure.UI.Blazor.csproj", "src/UI/Verdure.UI.Blazor/"]
COPY ["src/Services/Content/Verdure.Core/Verdure.Core.csproj", "src/Services/Content/Verdure.Core/"]
RUN dotnet restore "src/UI/Verdure.UI.Blazor/Verdure.UI.Blazor.csproj"
COPY . .
WORKDIR "/src/src/UI/Verdure.UI.Blazor"
RUN dotnet build "Verdure.UI.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Verdure.UI.Blazor.csproj" -c Release -o /app/publish

FROM nginx

EXPOSE 80

COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html/

#COPY --from=publish /app/publish/default.conf /etc/nginx/conf.d/default.conf

CMD [ "nginx", "-g", "daemon off;"]