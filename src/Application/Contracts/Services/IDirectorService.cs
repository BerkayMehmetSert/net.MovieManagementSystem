using Application.Contracts.Requests.Director;
using Application.Contracts.Responses;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IDirectorService
{
    void CreateDirector(CreateDirectorRequest request);
    void UpdateDirector(Guid id, UpdateDirectorRequest request);
    void DeleteDirector(Guid id);
    DirectorResponse GetDirectorById(Guid id);
    List<DirectorResponse> GetAllDirectors();
    Director GetDirectorEntityById(Guid id);
}