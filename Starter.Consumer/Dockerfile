FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine AS base

ENV APP_DIR=/app
ENV ENTRYPOINT="Starter.Consumer.dll"

COPY . $APP_DIR

WORKDIR $APP_DIR
ENTRYPOINT dotnet $ENTRYPOINT