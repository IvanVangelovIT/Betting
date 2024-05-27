using Domain.Features.Odds.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Features.Odds.Mappings
{
    public class OddMapping : IEntityTypeConfiguration<OddEntity>
    {
        public void Configure(EntityTypeBuilder<OddEntity> builder)
        {
            builder.ToTable("odds");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(o => o.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(o => o.Value)
                .HasColumnName("value")
                .IsRequired();

            builder.Property(o => o.SpecialValue)
                .HasColumnName("special_value");

            builder.Property(o => o.OddId)
                .HasColumnName("odd_id");
            
            builder.Property(o => o.BetId)
                .HasColumnName("bet_id");

            builder.HasOne(o => o.Bet)
                .WithMany(b => b.Odds)
                .HasForeignKey(o => o.BetId);
        }
    }
}