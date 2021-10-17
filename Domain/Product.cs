using System;

using Newtonsoft.Json;

using RefactorThis.Models;

namespace RefactorThis.Domain
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = true;
        }

        public Product(Guid id)
        {
            this.IsNew = true;
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"select * from Products where id = '{id}' collate nocase";

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                return;
            }

            this.IsNew = false;
            this.Id = Guid.Parse(rdr["Id"].ToString());
            this.Name = rdr["Name"].ToString();
            this.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
            this.Price = decimal.Parse(rdr["Price"].ToString());
            this.DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore] public bool IsNew { get; }

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
