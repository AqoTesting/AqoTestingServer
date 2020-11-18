using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.Infrastructure
{
    public class RedisConnectionConfig
    {
        public int Db { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public bool Ssl { get; set; }
    }
}
