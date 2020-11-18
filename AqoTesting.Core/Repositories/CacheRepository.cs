﻿using AqoTesting.Shared.Infrastructure;
using AqoTesting.Shared.Interfaces;
using BeetleX.Redis;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AqoTesting.Core.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IOptions<RedisConnectionConfig> _config;
        private RedisDB Redis { get; set; }
        public CacheRepository(IOptions<RedisConnectionConfig> config)
        {
            _config = config;

            Redis = new RedisDB(_config.Value.Db);
            Redis.Host.AddWriteHost(_config.Value.Host, _config.Value.Port, _config.Value.Ssl).Password = _config.Value.Password;
        }
        public async ValueTask<string> Set<T>(string key, T value, int seconds = 604800)
        {
            string json = JsonConvert.SerializeObject(value);
            return await Redis.Set(key, json, seconds);
        }
        public async ValueTask<T> Get<T>(string key, Func<T> createItem = null, int seconds = 604800)
        {
            string json = await Redis.Get<string>(key);
            T value = default;
            if (json != null) value = JsonConvert.DeserializeObject<T>(json);
            if (value == null && createItem != null)
            {
                value = createItem();
                await Set(key, value, seconds);
            }
            return value;
        }
        public async ValueTask<long> Exist(string key)
        {
            return await Redis.Exists(key);
        }
        public async Task<string[]> Keys(string pattern)
        {
            return await Redis.Keys(pattern);
        }
        public async ValueTask<long> Del(string key)
        {
            return await Redis.Del(key);
        }
        public async ValueTask<long> DelAll(string[] keys)
        {
            return await Redis.Del(keys);
        }
    }
}