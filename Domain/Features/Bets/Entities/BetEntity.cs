using Domain.Features.Matches.Entities;
using Domain.Features.Odds.Entities;

namespace Domain.Features.Bets.Entities;

public class BetEntity
{
    public int Id { get; set; }

    public int BetId { get; set; }
    
    public string Name { get; set; }
    
    public bool IsLive { get; set; }
    
    public int MatchId { get; set; }

    public MatchEntity Match { get; set; }
    
    public ICollection<OddEntity> Odds { get; set; }
}