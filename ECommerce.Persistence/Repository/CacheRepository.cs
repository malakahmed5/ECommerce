using ECommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repository
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetDataAsync(string cacheKey)
        {
            return await _database.StringGetAsync(cacheKey);
        }

        public async Task SetDataAsync(string cacheKey, string value, TimeSpan timeToLive)
        {
            await _database.StringSetAsync(cacheKey,value, timeToLive);
        }
    }
}
