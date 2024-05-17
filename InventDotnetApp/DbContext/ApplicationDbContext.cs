using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

/// <summary>
/// The database context for the application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    /// <param name="configuration">The configuration.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Gets or sets the products.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets or sets the stores.
    /// </summary>
    public DbSet<Store> Stores { get; set; }

    /// <summary>
    /// Gets or sets the inventory sales.
    /// </summary>
    public DbSet<InventorySale> InventorySales { get; set; }

    /// <summary>
    /// Configures the database context.
    /// </summary>
    /// <param name="optionsBuilder">The options builder.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                                ?? "Host=localhost;Database=invent-db;Username=postgres;Password=dbpassword";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
