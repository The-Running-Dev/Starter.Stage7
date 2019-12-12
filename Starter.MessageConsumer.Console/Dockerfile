FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine AS base

ARG artifacts

ENV HOME_DIR=/
ENV ENTRYPOINT="Starter.MessageConsumer.Console.dll"

RUN echo "Artifacts: $artifacts"
WORKDIR $artifacts
COPY . $HOME_DIR

ENTRYPOINT ["dotnet", $ENTRYPOINT]