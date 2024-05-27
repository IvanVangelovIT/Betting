using Domain.Features.Matches.MatchesChangeTracker;

namespace Domain.Features.Matches.Models;

public class MatchesChangeTracker
{
    public List<MatchChangeTracker> MatchesChanges { get; set; } = new List<MatchChangeTracker>();
}