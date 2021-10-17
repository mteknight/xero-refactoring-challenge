using System;

using Newtonsoft.Json;

using RefactorThis.Data;
using RefactorThis.Models;

namespace RefactorThis.Domain
{
    public class Product
    {
        public Product()
            : this(Guid.NewGuid())
        {
        }

        public Product(Guid id)
        {
            this.IsNew = true;
            this.Id = id;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore] public bool IsNew { get; private set; }

        public static Product Get(
            Guid id,
            IProductService service)
        {
            var product = service.Get(id);
            product.IsNew = false;

            return product;
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = this.IsNew
                ? $"insert into Products (id, name, description, price, deliveryprice) values ('{this.Id}', '{this.Name}', '{this.Description}', {this.Price}, {this.DeliveryPrice})"
                : $"update Products set name = '{this.Name}', description = '{this.Description}', price = {this.Price}, deliveryprice = {this.DeliveryPrice} where id = '{this.Id}' collate nocase";

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(this.Id).Items)
            {
                option.Delete();
            }

            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"delete from Products where id = '{this.Id}' collate nocase";
            cmd.ExecuteNonQuery();
        }
    }
}
