using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;

[TestFixture]
public class CsvImportServiceTests
{
    private Mock<ApplicationDbContext> _dbContextMock;
    private CsvImportService _csvImportService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var configurationMock = new Mock<IConfiguration>();
        _dbContextMock = new Mock<ApplicationDbContext>(options, configurationMock.Object);
        _csvImportService = new CsvImportService(_dbContextMock.Object);
    }

    [Test]
    public void ImportCsvData_ShouldImportProducts()
    {
        // Arrange
        var productsCsv = new List<ProductCsv>
        {
            new ProductCsv { ProductName = "Product1", Cost = 10.0m, SalesPrice = 20.0m }
        };
        var storesCsv = new List<StoreCsv>
        {
            new StoreCsv { StoreName = "Store1" }
        };
        var inventorySalesCsv = new List<InventorySaleCsv>
        {
            new InventorySaleCsv { ProductId = 1, StoreId = 1, Date = "2023-01-01", SalesQuantity = 5, Stock = 10 }
        };

        using (var writer = new StreamWriter("products.csv"))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteRecords(productsCsv);
        }

        using (var writer = new StreamWriter("stores.csv"))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteRecords(storesCsv);
        }

        using (var writer = new StreamWriter("inventory-sales.csv"))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteRecords(inventorySalesCsv);
        }

        // Act
        _csvImportService.ImportCsvData("products.csv", "stores.csv", "inventory-sales.csv");

        // Assert
        var products = _dbContextMock.Object.Products.ToList();
        Assert.That(products, Has.Count.EqualTo(1));
        Assert.That(products[0].ProductName, Is.EqualTo("Product1"));
    }
}
