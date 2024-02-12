FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY ["WeatherApi/WeatherApi.csproj", "WeatherApi/"]
COPY ["SharedModel/SharedModel.csproj", "SharedModel/"]
RUN dotnet restore "WeatherApi/WeatherApi.csproj"
COPY ["WeatherApi", "WeatherApi"]
COPY ["SharedModel", "SharedModel"]
WORKDIR "/app/WeatherApi"
RUN dotnet build "WeatherApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0  AS runtime
WORKDIR /app
COPY ["Https", "Https"]
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet","WeatherApi.dll"]