using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(string id);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetProductsByPriceAsync(decimal price);
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal min, decimal max);
        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
    }
}
