using Domain.Features.Bets.Entities;
using Domain.Features.Bets.Mappings;
using Domain.Features.Events.Entities;
using Domain.Features.Events.Mappings;
using Domain.Features.Matches.Entities;
using Domain.Features.Matches.Mappings;
using Domain.Features.Odds.Entities;
using Domain.Features.Odds.Mappings;
using Domain.Features.Sports.Entities;
using Domain.Features.Sports.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Domain.DbContexts;

public class BettingDbContext : DbContext
{
    public BettingDbContext(DbContextOptions<BettingDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<BetEntity> Bets { get; set; }
    
    public DbSet<EventEntity> Events { get; set; }
    
    public DbSet<MatchEntity> Matches { get; set; }
    
    public DbSet<OddEntity> Odds { get; set; }
    
    public DbSet<SportEntity> Sports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BetMapping());
        
        modelBuilder.ApplyConfiguration(new EventMapping());

        modelBuilder.ApplyConfiguration(new MatchMapping());
        
        modelBuilder.ApplyConfiguration(new OddMapping());
        
        modelBuilder.ApplyConfiguration(new SportMapping());
    }
}