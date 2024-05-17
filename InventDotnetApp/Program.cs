using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

// config for environment variables (DB connection string)
var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddDbContext<ApplicationDbContext>()
    .AddTransient<CsvImportService>()
    .AddTransient<SalesService>()
    .AddTransient<CommandExecutor>()
    .BuildServiceProvider();

var context = serviceProvider.GetService<ApplicationDbContext>();

if (!context.Database.CanConnect())
{
    Console.WriteLine("Cannot connect to the database.");
    return;
}

// to prevent unnecessary migrations
var tablesExist = context.Database.GetService<IRelationalDatabaseCreator>().Exists() &&
                  context.Database.GetService<IRelationalDatabaseCreator>().HasTables();

if (!tablesExist)
{
    context.Database.Migrate();
    Console.WriteLine("Database and tables created successfully.");
}


var executor = serviceProvider.GetService<CommandExecutor>();
executor.Execute(args);