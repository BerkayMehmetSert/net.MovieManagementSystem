using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration;

public class MovieRatingConfiguration : IEntityTypeConfiguration<MovieRating>
{
    public void Configure(EntityTypeBuilder<MovieRating> builder)
    {
        builder.ToTable("MovieRatings", "dbo");
        builder.HasKey(x => new { x.MovieId, x.RatingId });

        builder.HasOne(mr => mr.Movie)
            .WithMany(m => m.MovieRatings)
            .HasForeignKey(mr => mr.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mr => mr.Rating)
            .WithMany(r => r.MovieRatings)
            .HasForeignKey(mr => mr.RatingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}