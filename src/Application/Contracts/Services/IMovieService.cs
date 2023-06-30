using Application.Contracts.Requests.Movie;
using Application.Contracts.Responses;

namespace Application.Contracts.Services;

public interface IMovieService
{
    void CreateMovie(CreateMovieRequest request);
    void UpdateMovie(Guid id, UpdateMovieRequest request);
    void AddMovieActor(Guid id, AddMovieActorRequest request);
    void AddMovieCinema(Guid id, AddMovieCinemaRequest request);
    void AddMovieDirector(Guid id, AddMovieDirectorRequest request);
    void AddMovieGenre(Guid id, AddMovieGenreRequest request);
    void AddMovieLanguage(Guid id, AddMovieLanguageRequest request);
    void AddMovieRating(Guid id, AddMovieRatingRequest request);
    void DeleteMovie(Guid id);
    void RemoveMovieActor(Guid id, RemoveMovieActorRequest request);
    void RemoveMovieCinema(Guid id, RemoveMovieCinemaRequest request);
    void RemoveMovieDirector(Guid id, RemoveMovieDirectorRequest request);
    void RemoveMovieGenre(Guid id, RemoveMovieGenreRequest request);
    void RemoveMovieLanguage(Guid id, RemoveMovieLanguageRequest request);
    void RemoveMovieRating(Guid id, RemoveMovieRatingRequest request);
    MovieResponse GetMovieById(Guid id);
    MovieResponse GetMovieByTitle(string title);
    List<MovieResponse> GetAllMovies();
}