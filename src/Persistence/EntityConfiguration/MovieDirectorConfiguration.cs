using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration;

public class MovieDirectorConfiguration : IEntityTypeConfiguration<MovieDirector>
{
    public void Configure(EntityTypeBuilder<MovieDirector> builder)
    {
        builder.ToTable("MovieDirectors", "dbo");
        builder.HasKey(x => new { x.MovieId, x.DirectorId });

        builder.HasOne(md => md.Movie)
            .WithMany(m => m.MovieDirectors)
            .HasForeignKey(md => md.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(md => md.Director)
            .WithMany(d => d.MovieDirectors)
            .HasForeignKey(md => md.DirectorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}