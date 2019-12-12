FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS base

ARG ARTIFACTS

WORKDIR /

EXPOSE 80
EXPOSE 443

COPY ["$ARTIFACTS/", "."]

ENTRYPOINT ["dotnet", "Starter.API.dll"]