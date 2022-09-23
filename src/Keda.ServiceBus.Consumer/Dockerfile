FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine AS base
WORKDIR /app
RUN apk add --no-cache icu-libs icu-data-full
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["Keda.ServiceBus.Consumer.csproj", "./"]
RUN dotnet restore "Keda.ServiceBus.Consumer.csproj"
COPY . .
WORKDIR "/src/."

RUN dotnet build "Keda.ServiceBus.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Keda.ServiceBus.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Keda.ServiceBus.Consumer.dll"]