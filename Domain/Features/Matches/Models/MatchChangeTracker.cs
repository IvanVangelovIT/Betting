using Domain.Features.Matches.Models;

namespace Domain.Features.Matches.MatchesChangeTracker;

public class MatchChangeTracker
{
    public string SportType { get; set; }
    
    public string ModificationType { get; set; }
    
    public List<int> Ids { get; set; }
}