using Domain.DbContexts;
using Domain.Extensions;
using Domain.Features.Sports.Profiles;
using Domain.Features.Sports.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soap.Http;

namespace Domain;

public static class DependencyResolver
{
    public static IServiceCollection ApplySharedConfig(
         this IServiceCollection services,
         IConfiguration configuration)
     {
         AddBettingDbContext(services, configuration);
         
         services.AddAutoMapper(typeof(SportProfile));
         
         services.AddScoped<ParseXmlElements>();
         
         services.AddScoped<HttpService>();
         
         services.AddScoped<SportsChangeTrackerService>();
         
         services.AddScoped<SportsDbSaverService>();
         
         services.AddSingleton<RedisContext>();

         return services;
     }

     private static void AddBettingDbContext(IServiceCollection services, IConfiguration configuration)
     {
         var connectionString = configuration.GetBettingDbConnectionString();
         
         services.AddDbContext<BettingDbContext>(options =>
         {
             options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
         });
    }
}