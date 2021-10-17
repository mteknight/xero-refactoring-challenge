using System;

using Newtonsoft.Json;

using RefactorThis.Models;

namespace RefactorThis.Domain
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore] public bool IsNew { get; }

        public ProductOption()
        {
            this.Id = Guid.NewGuid();
            this.IsNew = true;
        }

        public ProductOption(Guid id)
        {
            this.IsNew = true;
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"select * from productoptions where id = '{id}' collate nocase";

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return;

            this.IsNew = false;
            this.Id = Guid.Parse(rdr["Id"].ToString());
            this.ProductId = Guid.Parse(rdr["ProductId"].ToString());
            this.Name = rdr["Name"].ToString();
            this.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = this.IsNew
                ? $"insert into productoptions (id, productid, name, description) values ('{this.Id}', '{this.ProductId}', '{this.Name}', '{this.Description}')"
                : $"update productoptions set name = '{this.Name}', description = '{this.Description}' where id = '{this.Id}' collate nocase";

            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"delete from productoptions where id = '{this.Id}' collate nocase";
            cmd.ExecuteReader();
        }
    }
}
