using Core.Application.Response;

namespace Application.Contracts.Responses;

public class MovieResponse : BaseResponse
{
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Plot { get; set; }
    public int MovieLength { get; set; }
    public ICollection<ActorResponse> Actors { get; set; }
    public ICollection<DirectorResponse> Directors { get; set; }
    public ICollection<CinemaResponse> Cinemas { get; set; }
    public ICollection<RatingResponse> Ratings { get; set; }
    public ICollection<GenreResponse> Genres { get; set; }

    public ICollection<LanguageResponse> Languages { get; set; }
}