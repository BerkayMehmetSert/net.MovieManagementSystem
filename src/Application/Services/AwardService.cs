using Application.Contracts.Constants.Award;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Award;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence;
using Domain.Entities;

namespace Application.Services;

public class AwardService : IAwardService
{
    private readonly IAwardRepository _awardRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AwardService(
        IAwardRepository awardRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _awardRepository = awardRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public void CreateAward(CreateAwardRequest request)
    {
        CheckIfAwardExistsByName(request.Name);
        var award = _mapper.Map<Award>(request);
        _awardRepository.Add(award);
        _unitOfWork.SaveChanges();
    }

    public void UpdateAward(Guid id, UpdateAwardRequest request)
    {
        var award = GetAwardEntityById(id);
        if (!string.Equals(award.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            CheckIfAwardExistsByName(request.Name);

        var updatedAward = _mapper.Map(request, award);
        _awardRepository.Update(updatedAward);
        _unitOfWork.SaveChanges();
    }

    public void DeleteAward(Guid id)
    {
        var award = GetAwardEntityById(id);
        if (award.ActorAwards is not null && award.ActorAwards.Any())
            throw new BusinessException(AwardBusinessMessages.AwardHasActors);

        _awardRepository.Delete(award);
        _unitOfWork.SaveChanges();
    }

    public AwardResponse GetAwardById(Guid id)
    {
        var award = GetAwardEntityById(id);
        return _mapper.Map<AwardResponse>(award);
    }

    public AwardResponse GetAwardByName(string name)
    {
        var award = _awardRepository.Get(predicate: x => x.Name.Equals(name));
        if (award is null)
            throw new NotFoundException(AwardBusinessMessages.AwardNotFoundByName);
        
        return _mapper.Map<AwardResponse>(award);
    }

    public List<AwardResponse> GetAllAwards()
    {
        var awards = _awardRepository.GetAll();
        return _mapper.Map<List<AwardResponse>>(awards);
    }

    public List<AwardResponse> GetAllAwardsOrderedByDateAsc()
    {
        var awards = _awardRepository.GetAll(orderBy: x => x.OrderBy(y => y.Date));
        return _mapper.Map<List<AwardResponse>>(awards);
    }

    public List<AwardResponse> GetAllAwardsOrderedByDateDesc()
    {
        var awards = _awardRepository.GetAll(orderBy: x => x.OrderByDescending(y => y.Date));
        return _mapper.Map<List<AwardResponse>>(awards);
    }

    public Award GetAwardEntityById(Guid id)
    {
        var award = _awardRepository.Get(predicate: x => x.Id.Equals(id));
        return award ?? throw new NotFoundException(AwardBusinessMessages.AwardNotFoundById);
    }

    private void CheckIfAwardExistsByName(string name)
    {
        var award = _awardRepository.Get(predicate: x => x.Name.Equals(name));
        if (award is not null)
            throw new BusinessException(AwardBusinessMessages.AwardAlreadyExistsByName);
    }
}