using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class RemoveMovieCinemaRequest : BaseRequest
{
    public virtual ICollection<Guid>? CinemaIds { get; set; }
}