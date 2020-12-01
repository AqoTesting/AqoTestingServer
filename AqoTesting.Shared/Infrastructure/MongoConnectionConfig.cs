namespace AqoTesting.Shared.Infrastructure
{
    public class MongoConnectionConfig
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string Host { get; set; }
        public ushort? Port { get; set; }
        public string? DefaultAuthDb { get; set; }
        public string? Options { get; set; }
    }
}
