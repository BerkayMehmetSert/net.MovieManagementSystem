using Application.Contracts.Services;
using Application.Contracts.Validations.Actor;
using Application.Contracts.Validations.Award;
using Application.Contracts.Validations.Cinema;
using Application.Contracts.Validations.Director;
using Application.Contracts.Validations.Genre;
using Application.Contracts.Validations.Language;
using Application.Contracts.Validations.Movie;
using Application.Contracts.Validations.Rating;
using Application.Contracts.Validations.User;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Core.Application.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Test.Extensions;

public static class TestExtensions
{
    public static void AddTestMockServices(this IServiceCollection services)
    {
        services.AddTransient<ActorFakeData>();
        services.AddTransient<AwardFakeData>();
        services.AddTransient<CinemaFakeData>();
        services.AddTransient<DirectorFakeData>();
        services.AddTransient<GenreFakeData>();
        services.AddTransient<LanguageFakeData>();
        services.AddTransient<RatingFakeData>();
        services.AddTransient<UserFakeData>();
        services.AddTransient<MovieFakeData>();
        services.AddTransient<IAwardService, AwardService>();
        services.AddTransient<ICacheService, MemoryCacheService>();
        
        services.AddTransient<CreateActorRequestValidator>();
        services.AddTransient<UpdateActorRequestValidator>();
        services.AddTransient<CreateAwardRequestValidator>();
        services.AddTransient<UpdateAwardRequestValidator>();
        services.AddTransient<CreateCinemaRequestValidator>();
        services.AddTransient<UpdateCinemaAddressRequestValidator>();
        services.AddTransient<UpdateCinemaNameRequestValidator>();
        services.AddTransient<CreateDirectorRequestValidator>();
        services.AddTransient<UpdateDirectorRequestValidator>();
        services.AddTransient<CreateGenreRequestValidator>();
        services.AddTransient<UpdateGenreRequestValidator>();
        services.AddTransient<CreateLanguageRequestValidator>();
        services.AddTransient<UpdateLanguageRequestValidator>();
        services.AddTransient<CreateRatingRequestValidator>();
        services.AddTransient<UpdateRatingRequestValidator>();
        services.AddTransient<UserLoginRequestValidation>();
        services.AddTransient<UserRegisterRequestValidator>();
        services.AddTransient<AdminLoginRequestValidator>();
        services.AddTransient<AdminRegisterRequestValidator>();
        services.AddTransient<ChangePasswordRequestValidation>();
        services.AddTransient<CreateMovieRequestValidator>();
        services.AddTransient<UpdateMovieRequestValidator>();
    }
}