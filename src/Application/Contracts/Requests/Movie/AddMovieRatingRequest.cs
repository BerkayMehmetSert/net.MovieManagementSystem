using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class AddMovieRatingRequest : BaseRequest
{
    public virtual ICollection<Guid>? RatingIds { get; set; }
}