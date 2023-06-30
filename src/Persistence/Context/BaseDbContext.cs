using System.Reflection;
using Core.Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Actor> Actors { get; set; }
    public DbSet<ActorAward> ActorAwards { get; set; }
    public DbSet<Award> Awards { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieActor> MovieActors { get; set; }
    public DbSet<MovieCinema> MovieCinemas { get; set; }
    public DbSet<MovieDirector> MovieDirectors { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<MovieLanguage> MovieLanguages { get; set; }
    public DbSet<MovieRating> MovieRatings { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Audit> Audits { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}