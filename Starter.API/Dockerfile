FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS base

ENV APP_DIR=/app
ENV ENTRYPOINT="Starter.API.dll"

COPY . $APP_DIR

EXPOSE 80
EXPOSE 443

WORKDIR $APP_DIR
ENTRYPOINT dotnet $ENTRYPOINT