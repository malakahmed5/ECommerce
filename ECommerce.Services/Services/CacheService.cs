using ECommerce.Domain.Contracts;
using ECommerce.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Services.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepo;

        public CacheService(ICacheRepository cacheRepo)
        {
            _cacheRepo = cacheRepo;
        }
        public async Task<string?> GetDataAsync(string cacheKey)
        {
            return await _cacheRepo.GetDataAsync(cacheKey);
        }

        public async Task SetDataAsync(string cacheKey, object value, TimeSpan timeToLive)
        {
            var cacheValue = JsonSerializer.Serialize(value);
            await _cacheRepo.SetDataAsync(cacheKey,cacheValue,timeToLive);
        }
    }
}
