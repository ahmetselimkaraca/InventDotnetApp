# Use the official .NET SDK image to build and publish the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app



# Copy the csproj files and restore any dependencies
COPY *.sln ./
COPY InventDotnetApp/*.csproj ./InventDotnetApp/
COPY InventDotnetApp.Tests/*.csproj ./InventDotnetApp.Tests/
RUN dotnet restore

# Copy the remaining files and publish the application
COPY InventDotnetApp/. ./InventDotnetApp/
COPY InventDotnetApp.Tests/. ./InventDotnetApp.Tests/
WORKDIR /app/InventDotnetApp
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/InventDotnetApp/out .

# Expose the port on which the application will run
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "InventDotnetApp.dll"]
