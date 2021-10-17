using System;
using System.Data;

using Dawn;

using RefactorThis.Domain;

namespace RefactorThis.Data
{
    public class ProductService : IProductService
    {
        private readonly IDataService dataService;

        public ProductService(IDataService dataService)
        {
            this.dataService = Guard.Argument(dataService, nameof(dataService)).NotNull().Value;
        }

        public Product Get(Guid id)
        {
            var connection = this.dataService.NewConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"select * from Products where id = '{id}' collate nocase";

            var reader = command.ExecuteReader();

            return reader.Read() ? GetProduct(id, reader) : new Product();
        }

        private static Product GetProduct(
            Guid id,
            IDataReader reader)
        {
            return new Product(id)
            {
                Name = reader["Name"].ToString(),
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                Price = decimal.Parse(reader["Price"].ToString()),
                DeliveryPrice = decimal.Parse(reader["DeliveryPrice"].ToString()),
            };
        }
    }
}
