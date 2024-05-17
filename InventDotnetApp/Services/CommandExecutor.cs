using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Executes commands based on the provided arguments.
/// </summary>
public class CommandExecutor
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutor"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public CommandExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the given command with the specified arguments.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public void Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No arguments provided.");
            return;
        }


        var command = args[0];
        switch (command)
        {
            case "--import-csv":
                ImportCsv();
                break;

            case "--list-sales":
                ListSales(args[1]);
                break;

            case "--add-sales":
                AddSales(args[1]);
                break;

            case "--delete-sales":
                DeleteSales(args[1]);
                break;

            case "--get-profit":
                GetProfit(args[1]);
                break;

            case "--get-most-profit":
                GetMostProfit();
                break;

            default:
                Console.WriteLine("Unknown command.");
                break;
        }
    }

    private void ImportCsv()
    {
        var csvImportService = _serviceProvider.GetService<CsvImportService>();
        csvImportService.ImportCsvData("Data/products.csv", "Data/stores.csv", "Data/inventory-sales.csv");
        Console.WriteLine("CSV data imported successfully.");
    }

    private void ListSales(string productIds)
    {
        if (string.IsNullOrEmpty(productIds))
        {
            Console.WriteLine("Usage: --list-sales <productIds>");
            return;
        }

        var salesService = _serviceProvider.GetService<SalesService>();
        salesService.ListSales(productIds);
    }

    private void AddSales(string salesData)
    {
        if (string.IsNullOrEmpty(salesData))
        {
            Console.WriteLine("Usage: --add-sales <salesData>");
            return;
        }

        var salesService = _serviceProvider.GetService<SalesService>();
        salesService.AddSales(salesData);
    }

    private void DeleteSales(string salesData)
    {
        if (string.IsNullOrEmpty(salesData))
        {
            Console.WriteLine("Usage: --delete-sales <salesData>");
            return;
        }

        var salesService = _serviceProvider.GetService<SalesService>();
        salesService.DeleteSales(salesData);
    }

    private void GetProfit(string storeIds)
    {
        if (string.IsNullOrEmpty(storeIds))
        {
            Console.WriteLine("Usage: --get-profit <storeIds>");
            return;
        }

        var salesService = _serviceProvider.GetService<SalesService>();
        salesService.GetProfit(storeIds);
    }

    private void GetMostProfit()
    {
        var salesService = _serviceProvider.GetService<SalesService>();
        salesService.GetMostProfit();
    }
}
