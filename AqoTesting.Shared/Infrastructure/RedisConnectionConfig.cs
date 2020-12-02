#pragma warning disable CS8618
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
