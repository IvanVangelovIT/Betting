using Domain.Features.Matches.Models;

namespace Domain.Features.Matches.MatchesChangeTracker;

public class MatchChangeTracker
{
    public Visibility Visibility { get; set; }
    
    public ChangeType ChangeType { get; set; }
    
    public List<int> Ids { get; set; }
}