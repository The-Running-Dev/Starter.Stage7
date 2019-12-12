FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine AS base

ARG artifacts

ENV HOME_DIR=/
ENV ENTRYPOINT="Starter.MessageConsumer.Console.dll"

COPY $artifacts $HOME_DIR

ENTRYPOINT ["dotnet", $ENTRYPOINT]