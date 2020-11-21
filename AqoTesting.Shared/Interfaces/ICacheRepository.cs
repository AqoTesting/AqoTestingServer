using System;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ICacheRepository
    {
        ValueTask<string> Set<T>(string key, T value, int seconds = 604800);
        ValueTask<T> Get<T>(string key, Func<T> createItem = null!, int seconds = 604800);
        ValueTask<long> Exist(string key);
        Task<string[]> Keys(string pattern);
        ValueTask<long> Del(string key);
        ValueTask<long> DelAll(string[] keys);
    }
}
