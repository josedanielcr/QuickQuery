using Microsoft.Extensions.Caching.Distributed;

namespace API.Utilities
{
    public class CacheUtils
    {
        private readonly IDistributedCache cache;

        public CacheUtils(IDistributedCache cache)
        {
            this.cache = cache;
        }
    }
}
