using Domain.Features.Bets.Entities;
using Domain.Features.Events.Entities;

namespace Domain.Features.Odds.Entities;

public class OddEntity
{
    public int Id { get; set; }
    
    public int OddId { get; set; }
    
    public string Name { get; set; }
    
    public decimal Value { get; set; }
    
    public decimal SpecialValue { get; set; }
    
    public int BetId { get; set; }

    public BetEntity Bet { get; set; }
}