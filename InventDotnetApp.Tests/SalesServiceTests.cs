using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

[TestFixture]
public class SalesServiceTests
{
    private ApplicationDbContext _dbContext;
    private SalesService _salesService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var configurationMock = new Mock<IConfiguration>();
        _dbContext = new ApplicationDbContext(options, configurationMock.Object);
        _salesService = new SalesService(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public void ListSales_ShouldPrintSales()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "Product1", Cost = 10.0m, SalesPrice = 20.0m };
        var store = new Store { Id = 1, StoreName = "Store1" };
        var inventorySale = new InventorySale
        {
            ProductId = 1,
            StoreId = 1,
            Date = new DateTime(2023, 1, 1),
            SalesQuantity = 5,
            Stock = 10
        };

        _dbContext.Products.Add(product);
        _dbContext.Stores.Add(store);
        _dbContext.InventorySales.Add(inventorySale);
        _dbContext.SaveChanges();

        // Act
        _salesService.ListSales("1");

        // Assert
        var sales = _dbContext.InventorySales.ToList();
        Assert.That(sales, Has.Count.EqualTo(1));
        Assert.That(sales[0].ProductId, Is.EqualTo(1));
    }

    [Test]
    public void AddSales_ShouldUpdateSales()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "Product1", Cost = 10.0m, SalesPrice = 20.0m };
        var store = new Store { Id = 1, StoreName = "Store1" };
        var inventorySale = new InventorySale
        {
            ProductId = 1,
            StoreId = 1,
            Date = new DateTime(2023, 1, 1),
            SalesQuantity = 5,
            Stock = 10
        };

        _dbContext.Products.Add(product);
        _dbContext.Stores.Add(store);
        _dbContext.InventorySales.Add(inventorySale);
        _dbContext.SaveChanges();

        // Act
        _salesService.AddSales("1,1,2023-01-01,2");

        // Assert
        var sales = _dbContext.InventorySales.ToList();
        Assert.That(sales, Has.Count.EqualTo(1));
        Assert.That(sales[0].SalesQuantity, Is.EqualTo(7));
        Assert.That(sales[0].Stock, Is.EqualTo(8)); // Check that stock has decreased correctly
    }
}
