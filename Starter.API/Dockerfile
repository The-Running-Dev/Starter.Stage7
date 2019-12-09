FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /src
COPY ["Starter.API/Starter.API.csproj", "Starter.API/"]
COPY ["Starter.Bootstrapper/Starter.Bootstrapper.csproj", "Starter.Bootstrapper/"]
COPY ["Starter.Data/Starter.Data.csproj", "Starter.Data/"]
COPY ["Starter.Framework/Starter.Framework.csproj", "Starter.Framework/"]
COPY ["Starter.MessageBroker.Azure/Starter.MessageBroker.Azure.csproj", "Starter.MessageBroker.Azure/"]
COPY ["Starter.Repository/Starter.Repository.csproj", "Starter.Repository/"]
RUN dotnet restore "Starter.API/Starter.API.csproj"
COPY . .
WORKDIR "/src/Starter.API"
RUN dotnet build "Starter.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Starter.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Starter.API.dll"]