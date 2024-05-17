# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY *.sln .
COPY InventDotnetApp/InventDotnetApp.csproj InventDotnetApp/
COPY InventDotnetApp.Tests/InventDotnetApp.Tests.csproj InventDotnetApp.Tests/
RUN dotnet restore

# Copy the remaining files and build the application
COPY InventDotnetApp/. InventDotnetApp/
COPY InventDotnetApp.Tests/. InventDotnetApp.Tests/

# Publish the application
WORKDIR /src/InventDotnetApp
RUN dotnet publish -c Release -o /app

# Use the official .NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Copy the Data directory containing CSV files to the runtime image
COPY InventDotnetApp/Data /app/Data

ENTRYPOINT ["dotnet"]
