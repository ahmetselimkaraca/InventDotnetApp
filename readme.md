# Invent Dotnet App

## Running with Docker

- Start the Docker engine and run `docker-compose build` (sometimes it fails to fetch .NET 8 SDK, in which case re-running the command usually works)
- After building is complete, run `docker-compose up`

- Run `docker-compose run invent-dotnet-app --import-csv` to import the CSV files to the postgres database
- With the prefix `docker-compose run invent-dotnet-app <args>`
  - `--list-sales "[productIds]"`: Gets the records for the given product list.
  - `--get-profit "[storeIds]"`: Gets the profit for a given store.
  - `--get-most-profit`: Gets the store with the highest profit.
  - `--add-sales "productId,storeId,date,salesQuantity"`: Adds new sales records for existing products and stores.
  - `--delete-sales "productId,storeId,date,salesQuantity"`: Deletes an existing sales record.

For unit tests, `docker-compose run tests` should also run unit tests but I wasn't able to fully implement it with proper tests for all cases.

## Running on Windows

- Set up a Postgres database with these credentials:

  - Database Name: invent-db
  - Username: postgres
  - Password: dbpassword

- Navigate to within the project file `cd InventDotnetApp`.
- Run `dotnet run --import-csv` to import the CSV files to the postgres database
- With the prefix `dotnet run <args>`
  - `--list-sales "[productIds]"`: Gets the records for the given product list.
  - `--get-profit "[storeIds]"`: Gets the profit for a given store.
  - `--get-most-profit`: Gets the store with the highest profit.
  - `--add-sales "productId,storeId,date,salesQuantity"`: Adds new sales records for existing products and stores.
  - `--delete-sales "productId,storeId,date,salesQuantity"`: Deletes an existing sales record.

To run tests, run `dotnet test` from the root directory where the .sln file resides.

## Design

The project is structured as a .NET Core app with a separate unit test project.

### Main Application (InventDotnetApp):

- Models: Define data entities such as Product, Store, and InventorySale.
- Services: Include CsvImportService for importing data from CSV files and SalesService for handling sales operations.
- DbContext: ApplicationDbContext for Entity Framework Core database context.
- Program: Entry point of the application that handles command-line arguments for different operations (e.g., importing CSV, listing sales).
  CommandExecutor: Encapsulates the command execution logic, making the Program class cleaner.

### Test Project (InventDotnetApp.Tests):

- Includes unit tests for the services and command execution using NUnit and Moq.
- Uses Entity Framework Core's in-memory provider for testing.

### Docker:

- Dockerfile for building and running the application.
- `docker-compose.yml` to orchestrate the application and PostgreSQL database.

**Notes**: The only manual interaction with the database is creating it. There are no SQL scripts, as table creation and data insertion are all done through the application using LINQ.
