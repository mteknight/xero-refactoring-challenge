using System.Data;
using System.Data.Common;

using AutoFixture.Xunit2;

using FluentAssertions;

using Moq;

using RefactorThis.Data;
using RefactorThis.Domain;

using Xunit;

namespace RefactorThis.Tests.Domain
{
    public sealed class ProductTests
    {
        [Theory]
        [AutoData]
        public void GivenNonExistentProduct_WhenCreatingProductById_ExpectStubProduct(Product product)
        {
            // Arrange
            var mockedSqliteDataReader = MockSqliteDataReader(product, false);
            var mockedSqlCommand = MockSqlCommand(mockedSqliteDataReader.Object);
            var mockedConnection = MockSqlConnection(mockedSqlCommand.Object);
            var mockedSqlLiteDataService = MockDataService(mockedConnection.Object);
            var productService = new ProductService(mockedSqlLiteDataService.Object);

            // Act
            var newProduct = Product.Get(product.Id, productService);

            // Assert
            newProduct.Should().NotBeNull();
        }

        [Theory]
        [AutoData]
        public void GivenExistingProduct_WhenCreatingProductById_ExpectObjectRetrieved(Product product)
        {
            // Arrange
            var mockedSqliteDataReader = MockSqliteDataReader(product);
            var mockedSqlCommand = MockSqlCommand(mockedSqliteDataReader.Object);
            var mockedConnection = MockSqlConnection(mockedSqlCommand.Object);
            var mockedSqlLiteDataService = MockDataService(mockedConnection.Object);
            var productService = new ProductService(mockedSqlLiteDataService.Object);

            // Act
            var newProduct = Product.Get(product.Id, productService);

            // Assert
            newProduct.Should().NotBeNull();
            newProduct.Id.Should().Be(product.Id);
            newProduct.Name.Should().Be(product.Name);
        }

        private static Mock<DbDataReader> MockSqliteDataReader(
            Product product,
            bool readResult = true)
        {
            var mockedSqliteDataReader = new Mock<DbDataReader>();
            mockedSqliteDataReader
                .Setup(reader => reader.Read())
                .Returns(true);

            mockedSqliteDataReader
                .Setup(reader => reader["Id"])
                .Returns(product.Id.ToString());

            mockedSqliteDataReader
                .Setup(reader => reader["Name"])
                .Returns(product.Name);

            mockedSqliteDataReader
                .Setup(reader => reader["Description"])
                .Returns(product.Description);

            mockedSqliteDataReader
                .Setup(reader => reader["Price"])
                .Returns(product.Price);

            mockedSqliteDataReader
                .Setup(reader => reader["DeliveryPrice"])
                .Returns(product.DeliveryPrice);

            return mockedSqliteDataReader;
        }

        private static Mock<IDbCommand> MockSqlCommand(DbDataReader mockedSqliteDataReader)
        {
            var mockedSqlCommand = new Mock<IDbCommand>();
            mockedSqlCommand
                .Setup(command => command.ExecuteReader())
                .Returns(mockedSqliteDataReader);

            return mockedSqlCommand;
        }

        private static Mock<IDbConnection> MockSqlConnection(IDbCommand mockedSqlCommand)
        {
            var mockedSqlConnection = new Mock<IDbConnection>();
            mockedSqlConnection
                .Setup(connection => connection.CreateCommand())
                .Returns(mockedSqlCommand);

            return mockedSqlConnection;
        }

        private static Mock<IDataService> MockDataService(IDbConnection mockedConnection)
        {
            var mockedSqlLiteDataService = new Mock<IDataService>();
            mockedSqlLiteDataService
                .Setup(service => service.NewConnection())
                .Returns(mockedConnection);

            return mockedSqlLiteDataService;
        }
    }
}
