FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS base

WORKDIR /

EXPOSE 80
EXPOSE 443

COPY Artificats/Starter.API/ .
RUN rm *.pdb

ENTRYPOINT ["dotnet", "Starter.API.dll"]