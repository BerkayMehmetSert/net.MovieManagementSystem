using Core.Application.Request;

namespace Application.Contracts.Requests.Movie;

public class UpdateMovieRequest : BaseRequest
{
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Plot { get; set; }
    public int MovieLength { get; set; }
}