FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS base

ARG artifacts

ENV HOME_DIR=/
ENV ENTRYPOINT="Starter.API.dll"

RUN echo "Artifacts: $artifacts"
WORKDIR $artifacts
COPY . $HOME_DIR

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", $ENTRYPOINT]