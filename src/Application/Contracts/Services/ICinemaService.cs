using Application.Contracts.Requests.Cinema;
using Application.Contracts.Responses;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface ICinemaService
{
    void CreateCinema(CreateCinemaRequest request);
    void UpdateCinemaAddress(Guid id, UpdateCinemaAddressRequest request);
    void UpdateCinemaName(Guid id, UpdateCinemaNameRequest request);
    void DeleteCinema(Guid id);
    CinemaResponse GetCinemaById(Guid id);
    CinemaResponse GetCinemaByName(string name);
    List<CinemaResponse> GetAllCinemas();
    Cinema GetCinemaEntityById(Guid id);
}