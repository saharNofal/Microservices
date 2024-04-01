namespace Play.common.Settings
{
    public class MongoDbsettings
    {
        public string? Host { get; init; }
        public int Port { get; init; }
        public string ConnectionString => $"mongodb://{Host}:{Port}";

    }
}