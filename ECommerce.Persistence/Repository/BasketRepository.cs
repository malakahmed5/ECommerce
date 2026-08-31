using AutoMapper.Configuration.Annotations;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModuleEntities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repository
{

    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasket(CustomerBasket basket , TimeSpan timeToLive = default)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            await _database.StringSetAsync(basket.Id, jsonBasket, (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);

            return await GetBasketById(basket.Id);
        }

        public async Task<bool> DeleteBasket(string basketId)
            => await _database.KeyDeleteAsync(basketId);

        public async Task<CustomerBasket?> GetBasketById(string basketId)
        {
            var basketAsJson = await _database.StringGetAsync(basketId);
            if(basketAsJson.IsNullOrEmpty)
                return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(basketAsJson!);
        }
    }
}
