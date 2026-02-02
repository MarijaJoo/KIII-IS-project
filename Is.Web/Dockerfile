FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Is.Web/Is.Web.csproj", "Is.Web/"]
COPY ["Is.Domain/Is.Domain.csproj", "Is.Domain/"]
COPY ["Is.Repository/Is.Repository.csproj", "Is.Repository/"]
COPY ["Is.Services/Is.Services.csproj", "Is.Services/"]
RUN dotnet restore "Is.Web/Is.Web.csproj"
COPY . .
WORKDIR "/src/Is.Web"
RUN dotnet publish "Is.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0.2 AS base
USER root
RUN apt-get update && \
    apt-get install -y libgssapi-krb5-2 && \
    rm -rf /var/lib/apt/lists/*

USER app
WORKDIR /app
EXPOSE 8080

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Is.Web.dll"]