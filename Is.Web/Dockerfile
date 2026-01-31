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

COPY publish/ .

ENTRYPOINT ["dotnet", "Is.Web.dll"]