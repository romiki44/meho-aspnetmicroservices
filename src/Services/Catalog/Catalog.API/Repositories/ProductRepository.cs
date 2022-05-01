using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext db;

        public ProductRepository(ICatalogContext catalogContext)
        {
            db = catalogContext;
        }

        public async Task<Product> GetProductById(string id)
        {
            return await db.Products.Find(p=>p.Id==id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await db.Products.Find(p=>true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await db.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await db.Products.Find(filter).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await db.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await db.Products.ReplaceOneAsync(g => g.Id == product.Id, product);
            return updateResult.IsAcknowledged && updateResult.MatchedCount > 0;  //lepsie MatchedCount ako ModifiedCount
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await db.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
