FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine AS base

WORKDIR /

COPY ${MESSAGE_CONSUMER_ARTIFACTS}/ .
RUN rm *.pdb

ENTRYPOINT ["dotnet", "Starter.MessageConsumer.Console.dll"]