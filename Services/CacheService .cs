using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            _cache.Set(key, value, expiration);
        }

        public T Get<T>(string key)
        {
            return _cache.TryGetValue(key, out T value) ? value : default;
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        // پیاده‌سازی متد GetVerificationCode
        public string GetVerificationCode(string phoneNumber)
        {
            return Get<string>(phoneNumber); // بازیابی کد از کش
        }

    }
}