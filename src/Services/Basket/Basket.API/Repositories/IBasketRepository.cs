using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string userName);
        Task DeleteBasketAsync(string userName);
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);
    }
}
