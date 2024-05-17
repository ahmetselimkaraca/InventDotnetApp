using System.Globalization;
using CsvHelper;

/// <summary>
/// Service for importing data from CSV files.
/// </summary>
public class CsvImportService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CsvImportService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public CsvImportService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Imports data from CSV files.
    /// </summary>
    /// <param name="productsFile">The products CSV file.</param>
    /// <param name="storesFile">The stores CSV file.</param>
    /// <param name="inventorySalesFile">The inventory sales CSV file.</param>
    public void ImportCsvData(string productsFile, string storesFile, string inventorySalesFile)
    {
        var products = ReadCsv<ProductCsv>(productsFile);
        var stores = ReadCsv<StoreCsv>(storesFile);
        var inventorySales = ReadCsv<InventorySaleCsv>(inventorySalesFile);

        foreach (var product in products)
        {
            _context.Products.Add(new Product
            {
                ProductName = product.ProductName,
                Cost = product.Cost,
                SalesPrice = product.SalesPrice
            });
        }

        foreach (var store in stores)
        {
            _context.Stores.Add(new Store
            {
                StoreName = store.StoreName
            });
        }

        foreach (var sale in inventorySales)
        {
            _context.InventorySales.Add(new InventorySale
            {
                ProductId = sale.ProductId,
                StoreId = sale.StoreId,
                Date = DateTime.SpecifyKind(DateTime.Parse(sale.Date), DateTimeKind.Utc),
                SalesQuantity = sale.SalesQuantity,
                Stock = sale.Stock
            });
        }

        _context.SaveChanges();
    }

    /// <summary>
    /// Reads data from a CSV file.
    /// </summary>
    /// <typeparam name="T">The type of records to read.</typeparam>
    /// <param name="filePath">The path to the CSV file.</param>
    /// <returns>A list of records read from the CSV file.</returns>
    private List<T> ReadCsv<T>(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().ToList();
        }
    }
}

/// <summary>
/// Represents a product record in the CSV file.
/// </summary>
public class ProductCsv
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Gets or sets the cost.
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// Gets or sets the sales price.
    /// </summary>
    public decimal SalesPrice { get; set; }
}

/// <summary>
/// Represents a store record in the CSV file.
/// </summary>
public class StoreCsv
{
    /// <summary>
    /// Gets or sets the store name.
    /// </summary>
    public string StoreName { get; set; }
}

/// <summary>
/// Represents an inventory sale record in the CSV file.
/// </summary>
public class InventorySaleCsv
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the store ID.
    /// </summary>
    public int StoreId { get; set; }

    /// <summary>
    /// Gets or sets the date of the sale.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// Gets or sets the quantity sold.
    /// </summary>
    public int SalesQuantity { get; set; }

    /// <summary>
    /// Gets or sets the stock after the sale.
    /// </summary>
    public int Stock { get; set; }
}
