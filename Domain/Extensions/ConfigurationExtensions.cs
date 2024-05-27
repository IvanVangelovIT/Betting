using Microsoft.Extensions.Configuration;

namespace Domain.Extensions;

public static class ConfigurationExtensions
{
    public static string GetBettingDbConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("Betting");
    
    public static string GetRedisDbConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("Redis");
    
    public static string GetHangfireConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("Hangfire");
}