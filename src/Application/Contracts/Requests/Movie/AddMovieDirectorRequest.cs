using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class AddMovieDirectorRequest : BaseRequest
{
    public virtual ICollection<Guid>? DirectorIds { get; set; }
}