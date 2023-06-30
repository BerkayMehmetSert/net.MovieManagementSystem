using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration;

public class MovieLanguageConfiguration : IEntityTypeConfiguration<MovieLanguage>
{
    public void Configure(EntityTypeBuilder<MovieLanguage> builder)
    {
        builder.ToTable("MovieLanguages", "dbo");
        builder.HasKey(x => new { x.MovieId, x.LanguageId });

        builder.HasOne(ml => ml.Movie)
            .WithMany(m => m.MovieLanguages)
            .HasForeignKey(ml => ml.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ml => ml.Language)
            .WithMany(l => l.MovieLanguages)
            .HasForeignKey(ml => ml.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}