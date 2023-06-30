using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class RemoveMovieGenreRequest : BaseRequest
{
    public virtual ICollection<Guid>? GenreIds { get; set; }
}