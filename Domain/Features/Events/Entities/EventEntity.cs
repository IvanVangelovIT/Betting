using Domain.Features.Matches.Entities;
using Domain.Features.Sports.Entities;

namespace Domain.Features.Events.Entities;

public class EventEntity
{
    public int Id { get; set; }
    
    public int EventId { get; set; }
    
    
    public string Name { get; set; }

    
    public bool IsLive { get; set; }
    
    public int CategoryId { get; set; }

    public int SportId { get; set; }

    public SportEntity Sport { get; set; }
    
    public ICollection<MatchEntity> Matches { get; set; }
}