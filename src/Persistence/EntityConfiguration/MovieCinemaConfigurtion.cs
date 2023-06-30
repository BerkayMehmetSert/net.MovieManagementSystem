using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration;

public class MovieCinemaConfigurtion : IEntityTypeConfiguration<MovieCinema>
{
    public void Configure(EntityTypeBuilder<MovieCinema> builder)
    {
        builder.ToTable("MovieCinemas", "dbo");
        builder.HasKey(x => new { x.MovieId, x.CinemaId });

        builder.HasOne(mc => mc.Movie)
            .WithMany(m => m.MovieCinemas)
            .HasForeignKey(mc => mc.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mc => mc.Cinema)
            .WithMany(c => c.MovieCinemas)
            .HasForeignKey(mc => mc.CinemaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}