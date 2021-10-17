using System;
using System.Collections.Generic;

using RefactorThis.Models;

namespace RefactorThis.Domain
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            this.LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            this.LoadProductOptions($"where productid = '{productId}' collate nocase");
        }

        private void LoadProductOptions(string where)
        {
            this.Items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"select id from productoptions {where}";

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr.GetString(0));
                this.Items.Add(new ProductOption(id));
            }
        }
    }
}
