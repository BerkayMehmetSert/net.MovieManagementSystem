using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class AddMovieLanguageRequest : BaseRequest
{
    public virtual ICollection<Guid>? LanguageIds { get; set; }
}