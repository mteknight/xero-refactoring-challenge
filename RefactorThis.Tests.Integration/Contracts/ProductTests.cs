using System;

using AutoFixture.Xunit2;

using FluentAssertions;

using Newtonsoft.Json;

using RefactorThis.Domain;

using Xunit;

namespace RefactorThis.Tests.Integration.Contracts
{
    public class ProductTests
    {
        [Theory]
        [AutoData]
        public void GivenValidJsonContract_WhenSerializingProduct_ExpectContractToMatch(
            ProductContract expectedContract)
        {
            // Arrange
            var product = (Product)expectedContract;

            // Act
            var expectedJson = JsonConvert.SerializeObject(expectedContract);
            var productJson = JsonConvert.SerializeObject(product);

            // Assert
            productJson.Should().Be(expectedJson, "Contract should match.");
        }
    }

    public record ProductContract(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        decimal DeliveryPrice)
    {
        public static explicit operator Product(ProductContract contract)
        {
            return new Product
            {
                Id = contract.Id,
                Name = contract.Name,
                Description = contract.Description,
                Price = contract.Price,
                DeliveryPrice = contract.DeliveryPrice
            };
        }
    }
}
