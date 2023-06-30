using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class RemoveMovieActorRequest : BaseRequest
{
    public virtual ICollection<Guid>? ActorIds { get; set; }
}