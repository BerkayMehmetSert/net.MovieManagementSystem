using Application.Contracts.Constants.Director;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Director;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence;
using Core.Utilities.Date;
using Domain.Entities;

namespace Application.Services;

public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository _directorRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DirectorService(
        IDirectorRepository directorRepository, 
        IMapper mapper, 
        IUnitOfWork unitOfWork
        )
    {
        _directorRepository = directorRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public void CreateDirector(CreateDirectorRequest request)
    {
        CheckIfDirectorDateOfBirthIsInTheFuture(request.DateOfBirth);
        var director = _mapper.Map<Director>(request);
        _directorRepository.Add(director);
        _unitOfWork.SaveChanges();
    }

    public void UpdateDirector(Guid id, UpdateDirectorRequest request)
    {
        var director = GetDirectorEntityById(id);
        CheckIfDirectorDateOfBirthIsInTheFuture(request.DateOfBirth);
        var updatedDirector = _mapper.Map(request, director);
        _directorRepository.Update(updatedDirector);
        _unitOfWork.SaveChanges();
    }

    public void DeleteDirector(Guid id)
    {
        var director = GetDirectorEntityById(id);
        _directorRepository.Delete(director);
        _unitOfWork.SaveChanges();
    }

    public DirectorResponse GetDirectorById(Guid id)
    {
        var director = GetDirectorEntityById(id);
        return _mapper.Map<DirectorResponse>(director);
    }

    public List<DirectorResponse> GetAllDirectors()
    {
        var directors = _directorRepository.GetAll();
        return _mapper.Map<List<DirectorResponse>>(directors);
    }

    public Director GetDirectorEntityById(Guid id)
    {
        var director = _directorRepository.Get(predicate: x => x.Id.Equals(id));
        return director ?? throw new NotFoundException(DirectorBusinessMessages.DirectorNotFoundById);
    }
    
    private static void CheckIfDirectorDateOfBirthIsInTheFuture(DateTime dateOfBirth)
    {
        if (dateOfBirth >= DateHelper.GetCurrentDate())
            throw new BusinessException(DirectorBusinessMessages.DirectorDateOfBirthIsInTheFuture);
    }
}