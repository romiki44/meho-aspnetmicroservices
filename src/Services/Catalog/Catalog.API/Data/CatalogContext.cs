using Catalog.API.AppConfig;
using Catalog.API.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IOptions<MongoDbSettings> dbOptions)
        {
            var client = new MongoClient(dbOptions.Value.ConnectionString);
            var database=client.GetDatabase(dbOptions.Value.DatabaseName);
            Products = database.GetCollection<Product>(dbOptions.Value.CollectionName);
            SeedDb.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
