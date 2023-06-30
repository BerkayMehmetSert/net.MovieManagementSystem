using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("Ratings", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Score).IsRequired().HasDefaultValue(0).HasPrecision(2, 1);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(500);

        builder.HasMany(r => r.MovieRatings)
            .WithOne(mr => mr.Rating)
            .HasForeignKey(mr => mr.RatingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}