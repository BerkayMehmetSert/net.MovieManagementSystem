using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration;

public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
{
    public void Configure(EntityTypeBuilder<Cinema> builder)
    {
        builder.ToTable("Cinemas", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(250);
        builder.Property(x => x.City).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Country).IsRequired().HasMaxLength(50);

        builder.HasIndex(x => x.Name).IsUnique();
        
        builder.HasMany(c => c.MovieCinemas)
            .WithOne(mc => mc.Cinema)
            .HasForeignKey(mc => mc.CinemaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}