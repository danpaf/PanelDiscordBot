FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MainBot/MainBot.csproj", "MainBot/"]
RUN dotnet restore "MainBot/MainBot.csproj"
COPY . .
WORKDIR "/src/MainBot"
RUN dotnet build "MainBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MainBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainBot.dll"]
