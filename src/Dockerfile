# Użyj oficjalnego obrazu .NET 8 SDK do budowania aplikacji
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Kopiowanie plików rozwiązania i przywrócenie zależności
COPY *.sln ./
COPY FakeStore.Presentation/FakeStore.Presentation.csproj ./FakeStore.Presentation/
COPY FakeStore.Business/FakeStore.Business.csproj ./FakeStore.Business/
COPY FakeStore.ApiClient/FakeStore.ApiClient.csproj ./FakeStore.ApiClient/
COPY Tests/FakeStore.Presentation.UnitTests/FakeStore.Presentation.UnitTests.csproj ./Tests/FakeStore.Presentation.UnitTests/
COPY Tests/FakeStore.Business.UnitTests/FakeStore.Business.UnitTests.csproj ./Tests/FakeStore.Business.UnitTests/
COPY Tests/FakeStore.ApiClient.UnitTests/FakeStore.ApiClient.UnitTests.csproj ./Tests/FakeStore.ApiClient.UnitTests/
RUN dotnet restore

# Kopiowanie reszty kodu i budowanie aplikacji
COPY . .
RUN dotnet publish FakeStore.sln -c Release -o /app/publish

# Użyj obrazu .NET 8 Runtime do uruchamiania aplikacji
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Otwórz port 80 dla ruchu HTTP
EXPOSE 80

# Uruchom aplikację
ENTRYPOINT ["dotnet", "Presentation.dll"]