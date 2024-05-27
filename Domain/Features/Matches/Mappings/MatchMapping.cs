using Domain.Features.Matches.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Features.Matches.Mappings
{
    public class MatchMapping : IEntityTypeConfiguration<MatchEntity>
    {
        public void Configure(EntityTypeBuilder<MatchEntity> builder)
        {
            builder.ToTable("matches");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(m => m.Type)
                .HasColumnName("type")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.StartDate)
                .HasColumnName("start_date")
                .IsRequired();
            
            builder.Property(m => m.MatchId)
                .HasColumnName("match_id");

            builder.Property(m => m.EventId)
                .HasColumnName("event_id");

            builder.HasOne(m => m.Event)
                .WithMany(e => e.Matches)
                .HasForeignKey(m => m.EventId);
            
            builder.HasIndex(m => m.MatchId);
        }
    }
}