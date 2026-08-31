using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetDataAsync(string cacheKey);
        Task SetDataAsync(string cacheKey, string value ,TimeSpan timeToLive);
    }
}
