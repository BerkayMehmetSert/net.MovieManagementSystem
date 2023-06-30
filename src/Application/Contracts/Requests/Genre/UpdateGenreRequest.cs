using Core.Application.Request;

namespace Application.Contracts.Requests.Genre;

public class UpdateGenreRequest : BaseRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}