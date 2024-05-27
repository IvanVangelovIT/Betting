using Domain.Features.Events.Entities;

namespace Domain.Features.Sports.Entities;

public class SportEntity
{
    public int Id { get; set; }
    
    public int SportId { get; set; }
    
    public string Name { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();
}