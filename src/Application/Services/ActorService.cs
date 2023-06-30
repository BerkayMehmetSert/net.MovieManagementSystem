using Application.Contracts.Constants.Actor;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Actor;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence;
using Core.Utilities.Date;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwardService _awardService;

    public ActorService(
        IActorRepository actorRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IAwardService awardService
    )
    {
        _actorRepository = actorRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _awardService = awardService;
    }

    public void CreateActor(CreateActorRequest request)
    {
        CheckIfActorDateOfBirthIsInTheFuture(request.DateOfBirth);
        var actor = _mapper.Map<Actor>(request);
        _actorRepository.Add(actor);
        _unitOfWork.SaveChanges();
    }

    public void UpdateActor(Guid id, UpdateActorRequest request)
    {
        var actor = GetActorEntityById(id);
        CheckIfActorDateOfBirthIsInTheFuture(request.DateOfBirth);
        var updatedActor = _mapper.Map(request, actor);
        _actorRepository.Update(updatedActor);
        _unitOfWork.SaveChanges();
    }

    public void AddActorAward(Guid id, AddActorAwardRequest request)
    {
        var actor = GetActorEntityById(id);
        var awards = GetAwards(request.AwardIds);

        actor.ActorAwards = awards.Select(x => new ActorAward
        {
            ActorId = actor.Id,
            AwardId = x.Id
        }).ToList();

        _actorRepository.Update(actor);
        _unitOfWork.SaveChanges();
    }

    public void RemoveActorAward(Guid id, RemoveActorAwardRequest request)
    {
        var actor = GetActorEntityById(id);
        var awards = GetAwards(request.AwardIds);

        actor.ActorAwards = actor.ActorAwards!
            .Where(x => !awards.Select(y => y.Id).Contains(x.AwardId))
            .ToList();

        _actorRepository.Update(actor);
        _unitOfWork.SaveChanges();
    }

    public void DeleteActor(Guid id)
    {
        var actor = GetActorEntityById(id);
        _actorRepository.Delete(actor);
        _unitOfWork.SaveChanges();
    }

    public ActorResponse GetActorById(Guid id)
    {
        var actor = GetActorEntityById(id);
        return _mapper.Map<ActorResponse>(actor);
    }

    public List<ActorResponse> GetAllActors()
    {
        var actors = _actorRepository.GetAll(
            include: source => source
                .Include(x => x.ActorAwards)
                .ThenInclude(x => x.Award)
        );

        return _mapper.Map<List<ActorResponse>>(actors);
    }

    public Actor GetActorEntityById(Guid id)
    {
        var actor = _actorRepository.Get(
            predicate: x => x.Id.Equals(id),
            include: source => source
                .Include(x => x.ActorAwards)
                .ThenInclude(x => x.Award)
        );
        return actor ?? throw new NotFoundException(ActorBusinessMessages.ActorNotFoundById);
    }

    private static void CheckIfActorDateOfBirthIsInTheFuture(DateTime dateOfBirth)
    {
        if (dateOfBirth >= DateHelper.GetCurrentDate())
            throw new BusinessException(ActorBusinessMessages.ActorDateOfBirthIsInTheFuture);
    }
    
    private List<Award> GetAwards(ICollection<Guid> awardIds)
    {
        return awardIds.Select(x => _awardService.GetAwardEntityById(x)).ToList();
    }
}