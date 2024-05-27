using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Domain.DbContexts;

public class BettingDbContextFactory : IDesignTimeDbContextFactory<BettingDbContext>
{
   public BettingDbContext CreateDbContext(string[] args)
   {
      var optionsBuilder = new DbContextOptionsBuilder<BettingDbContext>();
      
      optionsBuilder.UseMySql("Server=127.0.0.1;Port=3308;Database=Betting;Uid=root;", new MySqlServerVersion(new Version(8, 0, 23)));
       
      return new BettingDbContext(optionsBuilder.Options);
   }
}