using Application.Contracts.Repositories;
using Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceExtensions
{
    public static void AddPersistenceExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BaseDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MsSqlConnection")!);
        });
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IAwardRepository, AwardRepository>();
        services.AddScoped<ICinemaRepository, CinemaRepository>();
        services.AddScoped<IDirectorRepository, DirectorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}