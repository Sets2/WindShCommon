FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY ["WindSharing/Core/Core.csproj", "WindSharing/Core/"]
COPY ["WindSharing/DataAccess/DataAccess.csproj", "WindSharing/DataAccess/"]
COPY ["WindSharing/WebApi/WebApi.csproj", "WindSharing/WebApi/"]
COPY ["SharedModel/SharedModel.csproj", "SharedModel/"]
RUN dotnet restore "WindSharing/WebApi/WebApi.csproj"
COPY ["WindSharing/Core", "WindSharing/Core"]
COPY ["WindSharing/DataAccess", "WindSharing/DataAccess"]
COPY ["WindSharing/WebApi", "WindSharing/WebApi"]
COPY ["SharedModel", "SharedModel"]
RUN dotnet build "WindSharing/WebApi/WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WindSharing/WebApi/WebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0  AS runtime
WORKDIR /app
COPY ["Https", "Https"]
COPY ["WindSharing/WebApi/DirPhoto", "DirPhoto"]
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet","WebApi.dll"]
