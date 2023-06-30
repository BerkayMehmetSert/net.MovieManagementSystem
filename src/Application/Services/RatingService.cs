using Application.Contracts.Constants.Rating;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Rating;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence;
using Domain.Entities;

namespace Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RatingService(
        IRatingRepository ratingRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _ratingRepository = ratingRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public void CreateRating(CreateRatingRequest request)
    {
        var rating = _mapper.Map<Rating>(request);
        _ratingRepository.Add(rating);
        _unitOfWork.SaveChanges();
    }

    public void UpdateRating(Guid id, UpdateRatingRequest request)
    {
        var rating = GetRatingEntityById(id);
        var updatedRating = _mapper.Map(request, rating);
        _ratingRepository.Update(updatedRating);
        _unitOfWork.SaveChanges();
    }

    public void DeleteRating(Guid id)
    {
        var rating = GetRatingEntityById(id);
        _ratingRepository.Delete(rating);
        _unitOfWork.SaveChanges();
    }

    public RatingResponse GetRatingById(Guid id)
    {
        var rating = GetRatingEntityById(id);
        return _mapper.Map<RatingResponse>(rating);
    }

    public List<RatingResponse> GetAllRatings()
    {
        var ratings = _ratingRepository.GetAll();
        return _mapper.Map<List<RatingResponse>>(ratings);
    }

    public Rating GetRatingEntityById(Guid id)
    {
        var rating = _ratingRepository.Get(predicate: x => x.Id.Equals(id));
        return rating ?? throw new NotFoundException(RatingBusinessMessages.RatingNotFoundById);
    }
}