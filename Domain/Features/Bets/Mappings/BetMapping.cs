using Domain.Features.Bets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Features.Bets.Mappings
{
    public class BetMapping : IEntityTypeConfiguration<BetEntity>
    {
        public void Configure(EntityTypeBuilder<BetEntity> builder)
        {
            builder.ToTable("bets");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(b => b.IsLive)
                .HasColumnName("is_live")
                .IsRequired();

            builder.Property(b => b.MatchId)
                .HasColumnName("match_id");
            
            builder.Property(b => b.BetId)
                .HasColumnName("bet_id");

            builder
                .HasOne(b => b.Match)
                .WithMany(m => m.Bets)
                .HasForeignKey(b => b.MatchId);
        }
    }
}