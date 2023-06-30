using System.Reflection;
using Application.Contracts.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationExtensions
{
    public static void AddApplicationExtensions(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IAwardService, AwardService>();
        services.AddScoped<ICinemaService, CinemaService>();
        services.AddScoped<IDirectorService, DirectorService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
    }
}