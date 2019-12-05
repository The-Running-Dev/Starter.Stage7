FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Starter.MessageConsumer.Console/Starter.MessageConsumer.Console.csproj", "Starter.MessageConsumer.Console/"]
RUN dotnet restore "Starter.MessageConsumer.Console/Starter.MessageConsumer.Console.csproj"
COPY . .
WORKDIR "/src/Starter.MessageConsumer.Console"
RUN dotnet build "Starter.MessageConsumer.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Starter.MessageConsumer.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Starter.MessageConsumer.Console.dll"]