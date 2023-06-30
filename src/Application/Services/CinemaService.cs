using Application.Contracts.Constants.Cinema;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Cinema;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence;
using Domain.Entities;

namespace Application.Services;

public class CinemaService : ICinemaService
{
    private readonly ICinemaRepository _cinemaRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CinemaService(
        ICinemaRepository cinemaRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _cinemaRepository = cinemaRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public void CreateCinema(CreateCinemaRequest request)
    {
        CheckIfCinemaExistsByName(request.Name);
        var cinema = _mapper.Map<Cinema>(request);
        _cinemaRepository.Add(cinema);
        _unitOfWork.SaveChanges();
    }

    public void UpdateCinemaAddress(Guid id, UpdateCinemaAddressRequest request)
    {
        var cinema = GetCinemaEntityById(id);
        var updatedCinema = _mapper.Map(request, cinema);
        _cinemaRepository.Update(updatedCinema);
        _unitOfWork.SaveChanges();
    }

    public void UpdateCinemaName(Guid id, UpdateCinemaNameRequest request)
    {
        var cinema = GetCinemaEntityById(id);
        if (!string.Equals(cinema.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            CheckIfCinemaExistsByName(request.Name);
        
        var updatedCinema = _mapper.Map(request, cinema);
        _cinemaRepository.Update(updatedCinema);
        _unitOfWork.SaveChanges();
    }

    public void DeleteCinema(Guid id)
    {
        var cinema = GetCinemaEntityById(id);
        if (cinema.MovieCinemas is not null && cinema.MovieCinemas.Any())
            throw new BusinessException(CinemaBusinessMessages.CinemaHasMovies);
        
        _cinemaRepository.Delete(cinema);
        _unitOfWork.SaveChanges();
    }

    public CinemaResponse GetCinemaById(Guid id)
    {
        var cinema = GetCinemaEntityById(id);
        return _mapper.Map<CinemaResponse>(cinema);
    }

    public CinemaResponse GetCinemaByName(string name)
    {
        var cinema = _cinemaRepository.Get(predicate: x => x.Name.Equals(name));
        if (cinema is null)
            throw new NotFoundException(CinemaBusinessMessages.CinemaNotFoundByName);
        return _mapper.Map<CinemaResponse>(cinema);
    }

    public List<CinemaResponse> GetAllCinemas()
    {
        var cinemas = _cinemaRepository.GetAll();
        return _mapper.Map<List<CinemaResponse>>(cinemas);
    }

    public Cinema GetCinemaEntityById(Guid id)
    {
        var cinema = _cinemaRepository.Get(predicate: x => x.Id.Equals(id));
        return cinema ?? throw new NotFoundException(CinemaBusinessMessages.CinemaNotFoundById);
    }

    private void CheckIfCinemaExistsByName(string name)
    {
        var cinema = _cinemaRepository.Get(predicate: x => x.Name.Equals(name));
        if (cinema is not null)
            throw new BusinessException(CinemaBusinessMessages.CinemaAlreadyExists);
    }
}