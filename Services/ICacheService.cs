using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public interface ICacheService
    {
        void Set<T>(string key, T value, TimeSpan expiration);
        T Get<T>(string key);
        bool TryGetValue<T>(string key, out T value);


        // متد جدید برای بازیابی کد تأیید
        string GetVerificationCode(string phoneNumber);
    }
}