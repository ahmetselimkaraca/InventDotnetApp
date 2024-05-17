using Microsoft.EntityFrameworkCore;

/// <summary>
/// Service for handling sales operations.
/// </summary>
public class SalesService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SalesService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lists sales for the specified product IDs.
    /// </summary>
    /// <param name="productIds">A comma-separated list of product IDs.</param>
    public void ListSales(string productIds)
    {
        // Convert the comma-separated string to a list of integers
        var ids = productIds.Split(',').Select(int.Parse).ToList();

        // Query the database for sales data
        var sales = _context.InventorySales
            .Where(s => ids.Contains(s.ProductId))
            .Select(s => new
            {
                _context.Products.FirstOrDefault(p => p.Id == s.ProductId).ProductName,
                _context.Stores.FirstOrDefault(st => st.Id == s.StoreId).StoreName,
                s.Date,
                s.SalesQuantity,
                s.Stock
            })
            .ToList();

        Console.WriteLine("ProductName,StoreName,Date,SalesQuantity,Stock");
        foreach (var sale in sales)
        {
            Console.WriteLine($"{sale.ProductName},{sale.StoreName},{sale.Date},{sale.SalesQuantity},{sale.Stock}");
        }
    }

    /// <summary>
    /// Adds sales data.
    /// </summary>
    /// <param name="salesData">The sales data in the format "productId,storeId,date,salesQuantity".</param>
    public void AddSales(string salesData)
    {
        var data = salesData.Split(',');
        var productId = int.Parse(data[0]);
        var storeId = int.Parse(data[1]);
        var date = DateTime.Parse(data[2]);
        var salesQuantity = int.Parse(data[3]);

        // based on the sales data, this query always returns a unique record.
        // No item has its stock information updated twice on the same day at
        // the same store. Had this been the case, we would have to query the
        // LAST sale of that item as it would have the most recent stock.
        var existingSale = _context.InventorySales.FirstOrDefault(s => s.ProductId == productId && s.StoreId == storeId && s.Date == date);
        if (existingSale != null && existingSale.Stock >= salesQuantity)
        {
            // updating quantity and stock
            existingSale.SalesQuantity += salesQuantity;
            existingSale.Stock -= salesQuantity;
            _context.SaveChanges();
            Console.WriteLine("Sales record updated.");
        }
        else
        {
            Console.WriteLine("Sales record not found or insufficient stock.");
        }
    }

    /// <summary>
    /// Deletes sales data.
    /// </summary>
    /// <param name="salesData">The sales data in the format "productId,storeId,date".</param>
    public void DeleteSales(string salesData)
    {
        var data = salesData.Split(',');
        var productId = int.Parse(data[0]);
        var storeId = int.Parse(data[1]);
        var date = DateTime.Parse(data[2]);

        var sale = _context.InventorySales.FirstOrDefault(s => s.ProductId == productId && s.StoreId == storeId && s.Date == date);
        if (sale != null)
        {
            _context.InventorySales.Remove(sale);
            _context.SaveChanges();
            Console.WriteLine("Sales record deleted.");
        }
        else
        {
            Console.WriteLine("Sales record not found.");
        }
    }

    /// <summary>
    /// Gets the profit for the specified store IDs.
    /// </summary>
    /// <param name="storeIds">A comma-separated list of store IDs.</param>
    public void GetProfit(string storeIds)
    {
        var ids = storeIds.Split(',').Select(int.Parse).ToList();
        var profits = _context.InventorySales
            .Where(s => ids.Contains(s.StoreId))
            .GroupBy(s => s.StoreId)
            .Select(g => new
            {
                // profit = sum of (sales quantity * (sales price - cost))
                _context.Stores.FirstOrDefault(st => st.Id == g.Key).StoreName,
                Profit = g.Sum(s => s.SalesQuantity * (_context.Products.FirstOrDefault(p => p.Id == s.ProductId).SalesPrice - _context.Products.FirstOrDefault(p => p.Id == s.ProductId).Cost))
            })
            .ToList();

        Console.WriteLine("StoreName,Profit");
        foreach (var profit in profits)
        {
            Console.WriteLine($"{profit.StoreName},{profit.Profit}");
        }
    }

    /// <summary>
    /// Gets the store with the most profit.
    /// </summary>
    public void GetMostProfit()
    {
        var mostProfitableStore = _context.InventorySales
            .GroupBy(s => s.StoreId)
            .Select(g => new
            {
                _context.Stores.FirstOrDefault(st => st.Id == g.Key).StoreName,
                Profit = g.Sum(s => s.SalesQuantity * (_context.Products.FirstOrDefault(p => p.Id == s.ProductId).SalesPrice - _context.Products.FirstOrDefault(p => p.Id == s.ProductId).Cost))
            })
            .OrderByDescending(s => s.Profit)
            .FirstOrDefault();

        Console.WriteLine("StoreName");
        if (mostProfitableStore != null)
        {
            Console.WriteLine($"{mostProfitableStore.StoreName}");
        }
    }
}
