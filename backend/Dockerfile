# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Mojica.Api/Mojica.Api.csproj Mojica.Api/
RUN dotnet restore Mojica.Api/Mojica.Api.csproj

COPY Mojica.Api/ Mojica.Api/
RUN dotnet publish Mojica.Api/Mojica.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

USER app

ENTRYPOINT ["dotnet", "Mojica.Api.dll"]
