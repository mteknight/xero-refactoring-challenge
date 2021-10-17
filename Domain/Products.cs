using System;
using System.Collections.Generic;

using RefactorThis.Models;

namespace RefactorThis.Domain
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            this.LoadProducts(null);
        }

        public Products(string name)
        {
            this.LoadProducts($"where lower(name) like '%{name.ToLower()}%'");
        }

        private void LoadProducts(string where)
        {
            this.Items = new List<Product>();
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"select id from Products {where}";

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr.GetString(0));
                this.Items.Add(new Product(id));
            }
        }
    }
}
