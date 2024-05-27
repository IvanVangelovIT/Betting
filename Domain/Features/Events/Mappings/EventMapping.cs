using Domain.Features.Events.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Features.Events.Mappings;

public class EventMapping : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder
            .ToTable("events");

        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(s => s.EventId)
            .HasColumnName("event_id");
        
        builder.Property(s => s.Name)
            .HasColumnName("name");
        
        builder.Property(s => s.IsLive)
            .HasColumnName("is_live");
        
        builder.Property(s => s.CategoryId)
            .HasColumnName("category_id");
    }
}