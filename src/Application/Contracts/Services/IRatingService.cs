using Application.Contracts.Requests.Rating;
using Application.Contracts.Responses;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IRatingService
{
    void CreateRating(CreateRatingRequest request);
    void UpdateRating(Guid id, UpdateRatingRequest request);
    void DeleteRating(Guid id);
    RatingResponse GetRatingById(Guid id);
    List<RatingResponse> GetAllRatings();
    Rating GetRatingEntityById(Guid id);
}