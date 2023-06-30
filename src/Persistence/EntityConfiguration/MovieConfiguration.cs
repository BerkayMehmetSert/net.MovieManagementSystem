using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ReleaseDate).IsRequired();
        builder.Property(x => x.Plot).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.MovieLength).IsRequired();

        builder.HasIndex(x => x.Title).IsUnique();
        
        builder.HasMany(m => m.MovieActors)
            .WithOne(ma => ma.Movie)
            .HasForeignKey(ma => ma.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.MovieDirectors)
            .WithOne(md => md.Movie)
            .HasForeignKey(md => md.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.MovieGenres)
            .WithOne(mg => mg.Movie)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.MovieLanguages)
            .WithOne(ml => ml.Movie)
            .HasForeignKey(ml => ml.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.MovieCinemas)
            .WithOne(mc => mc.Movie)
            .HasForeignKey(mc => mc.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.MovieRatings)
            .WithOne(mr => mr.Movie)
            .HasForeignKey(mr => mr.MovieId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}