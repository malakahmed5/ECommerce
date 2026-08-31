using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface ICacheService
    {
        Task<string?> GetDataAsync(string cacheKey);
        Task SetDataAsync (string cacheKey, object value , TimeSpan timeToLive);
    }
}
