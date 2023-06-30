using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class RemoveMovieLanguageRequest : BaseRequest
{
    public virtual ICollection<Guid>? LanguageIds { get; set; }
}