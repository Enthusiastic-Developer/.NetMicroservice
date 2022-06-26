using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _radisCache;

        public BasketRepository(IDistributedCache radisCache)
        {
            _radisCache = radisCache ?? throw new System.ArgumentNullException(nameof(radisCache));
        }

        public async Task DeleteBasketAsync(string userName)
        {
            await _radisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            var basket = await _radisCache.GetStringAsync(userName);
            if (String.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            await _radisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasketAsync(basket.UserName!);
        }
    }
}
