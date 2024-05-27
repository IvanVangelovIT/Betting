using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Domain.DbContexts;

public class RedisContext
{
    private readonly IConfiguration _config;
    private readonly ConnectionMultiplexer _connection;

    public RedisContext(IConfiguration config)
    {
        _config = config;
        _connection = ConnectionMultiplexer.Connect("localhost:6379");
    }

    public IDatabase GetDatabase()
    {
        return _connection.GetDatabase();
    }
}