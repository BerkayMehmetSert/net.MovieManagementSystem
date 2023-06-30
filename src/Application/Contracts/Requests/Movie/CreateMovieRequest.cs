using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class CreateMovieRequest : BaseRequest
{
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Plot { get; set; }
    public int MovieLength { get; set; }
    public virtual ICollection<Guid>? ActorIds { get; set; }
    public virtual ICollection<Guid>? DirectorIds { get; set; }
    public virtual ICollection<Guid>? CinemaIds { get; set; }
    public virtual ICollection<Guid>? RatingIds { get; set; }
    public virtual ICollection<Guid>? GenreIds { get; set; }
    public virtual ICollection<Guid>? LanguageIds { get; set; }
}