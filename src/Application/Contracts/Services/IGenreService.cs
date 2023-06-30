using Application.Contracts.Requests.Genre;
using Application.Contracts.Responses;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IGenreService
{
    void CreateGenre(CreateGenreRequest request);
    void UpdateGenre(Guid id, UpdateGenreRequest request);
    void DeleteGenre(Guid id);
    GenreResponse GetGenreById(Guid id);
    GenreResponse GetGenreByName(string name);
    List<GenreResponse> GetAllGenres();
    Genre GetGenreEntityById(Guid id);
}