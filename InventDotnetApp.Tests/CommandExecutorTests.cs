using NUnit.Framework;
using Moq;
using Microsoft.Extensions.DependencyInjection;

[TestFixture]
public class CommandExecutorTests
{
    private Mock<IServiceProvider> _serviceProviderMock;
    private CommandExecutor _commandExecutor;

    [SetUp]
    public void Setup()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _commandExecutor = new CommandExecutor(_serviceProviderMock.Object);
    }

    [Test]
    public void Execute_ShouldCallImportCsv()
    {
        // Arrange
        var csvImportServiceMock = new Mock<CsvImportService>(null);
        _serviceProviderMock.Setup(x => x.GetService(typeof(CsvImportService))).Returns(csvImportServiceMock.Object);

        // Act
        _commandExecutor.Execute(new[] { "--import-csv" });

        // Assert
        csvImportServiceMock.Verify(x => x.ImportCsvData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void Execute_ShouldCallListSales()
    {
        // Arrange
        var salesServiceMock = new Mock<SalesService>(null);
        _serviceProviderMock.Setup(x => x.GetService(typeof(SalesService))).Returns(salesServiceMock.Object);

        // Act
        _commandExecutor.Execute(new[] { "--list-sales", "1" });

        // Assert
        salesServiceMock.Verify(x => x.ListSales(It.IsAny<string>()), Times.Once);
    }
}
