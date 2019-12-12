FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine AS base

ARG ARTIFACTS

WORKDIR /

COPY $ARTIFACTS/* .
RUN rm $ARTIFACTS/*.pdb

ENTRYPOINT ["dotnet", "Starter.MessageConsumer.Console.dll"]