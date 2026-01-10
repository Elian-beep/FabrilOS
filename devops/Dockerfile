FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FabrilOS.API/FabrilOS.API.csproj", "FabrilOS.API/"]

RUN dotnet restore "FabrilOS.API/FabrilOS.API.csproj"

COPY . .

WORKDIR "/src/FabrilOS.API"
RUN dotnet build "FabrilOS.API.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "FabrilOS.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FabrilOS.API.dll"]