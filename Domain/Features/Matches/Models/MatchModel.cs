using Soap.Models;

namespace Domain.Features.Matches.Models;

public class MatchModel
{
    public int MatchId { get; set; }
    
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public ICollection<BetXmlModel> Bets { get; set; }
}