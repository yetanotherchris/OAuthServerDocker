﻿FROM mcr.microsoft.com/dotnet/aspnet:5.0
MAINTAINER Chris Small

RUN mkdir /app
COPY ./bin/Release/netcoreapp5.0/publish /app
WORKDIR /app

ENTRYPOINT ["dotnet", "/app/OAuthServer.IdentityServer.dll"]

# ASP.NET Core: Kestrel should listen on all IPs
ENV ASPNETCORE_URLS="https://0.0.0.0:5000"
ENV ASPNETCORE_ENVIRONMENT="Production"

# Steps for building:
#dotnet publish -c Release
#docker build -t "oauthserver-identityserver" .
#docker run -p 5000:5000 oauthserver-identityserver
