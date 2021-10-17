using System;

using AutoFixture.Xunit2;

using FluentAssertions;

using Moq;

using RefactorThis.Controllers;
using RefactorThis.Data;
using RefactorThis.Domain;

using Xunit;

namespace RefactorThis.Tests.Controllers
{
    public sealed class ProductsControllerTests
    {
        [Theory]
        [AutoData]
        public void GivenExistingProduct_WhenGetProductById_ExpectProductNotNew(
            Product product)
        {
            // Arrange
            var mockedProductService = new Mock<IProductService>();
            mockedProductService
                .Setup(service => service.Get(product.Id))
                .Returns(product);

            var sut = new ProductsController(mockedProductService.Object);

            // Act
            Action SutCall = () => sut.Get(product.Id);

            // Assert
            SutCall.Should().NotThrow<Exception>();
        }
    }
}
