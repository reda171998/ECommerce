using StackExchange.Redis;

namespace Basket.Api.Data.Interfaces
{
    public interface IBasketContext
    {

        public IDatabase Redis { get; }
    }
}
