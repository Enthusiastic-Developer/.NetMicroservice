using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult result = await _context.Products.DeleteOneAsync(x => x.Id == id);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await _context
                .Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context
                .Products
                .Find(x => true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await _context
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceAsync(decimal price)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Price, price);
            return await _context
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal min, decimal max)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Gte(p => p.Price, min) & Builders<Product>.Filter.Lte(p => p.Price, max);
            return await _context
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context
                 .Products
                 .ReplaceOneAsync(
                     filter: g => g.Id == product.Id,
                     replacement: product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
