using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class RemoveMovieRatingRequest : BaseRequest
{
    public virtual ICollection<Guid>? RatingIds { get; set; }
}