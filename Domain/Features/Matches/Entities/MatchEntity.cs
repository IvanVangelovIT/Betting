using Domain.Features.Bets.Entities;
using Domain.Features.Events.Entities;

namespace Domain.Features.Matches.Entities;

public class MatchEntity
{
    public int Id { get; set; }
    
    public int MatchId { get; set; } // Added index
    
    public string Name { get; set; }
    
    public string Type { get; set; }
    
    public DateTime StartDate { get; set; }

    public int EventId { get; set; }
    
    public EventEntity Event { get; set; }
    
    public ICollection<BetEntity> Bets { get; set; }
}