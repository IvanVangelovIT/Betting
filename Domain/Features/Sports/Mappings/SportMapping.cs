using Domain.Features.Sports.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Features.Sports.Mappings
{
    public class SportMapping : IEntityTypeConfiguration<SportEntity>
    {
        public void Configure(EntityTypeBuilder<SportEntity> builder)
        {
            builder.ToTable("sports");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(s => s.SportId)
                .HasColumnName("sport_id");

            builder.Property(s => s.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(s => s.CreatedOn)
                .HasColumnName("created_on");

            builder.HasMany(s => s.Events)
                .WithOne(e => e.Sport)
                .HasForeignKey(e => e.SportId);
        }
    }
}