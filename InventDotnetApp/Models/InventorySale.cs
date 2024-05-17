/// <summary>
/// Represents an inventory sale.
/// </summary>
public class InventorySale
{
    /// <summary>
    /// Gets or sets the ID of the inventory sale.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the product.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the store.
    /// </summary>
    public int StoreId { get; set; }

    /// <summary>
    /// Gets or sets the date of the sale.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the quantity sold.
    /// </summary>
    public int SalesQuantity { get; set; }

    /// <summary>
    /// Gets or sets the stock after the sale.
    /// </summary>
    public int Stock { get; set; }
}
