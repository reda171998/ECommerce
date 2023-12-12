using Basket.Api.Data.Interfaces;
using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Newtonsoft.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository:IBasketRepository
    {
        private readonly IBasketContext _context;
        public BasketRepository(IBasketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> DeleteBasket(string userName)
        {
            return await _context.Redis.KeyDeleteAsync(userName);
        }

        public async Task<BasketCart?> GetBasket(string userName)
        {
            var basket = await _context.Redis.StringGetAsync(userName);
            if (basket.IsNullOrEmpty)
                return null;
            return JsonConvert.DeserializeObject<BasketCart>(basket!);
        }

        public async Task<BasketCart?> UpdateBasket(BasketCart cart)
        {
            var updated = await _context.Redis.StringSetAsync(cart.UserName, JsonConvert.SerializeObject(cart));
            if (!updated)
                return null;
            return await GetBasket(cart.UserName);
        }
    }
}

