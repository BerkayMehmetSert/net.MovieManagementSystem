using Core.Application.Request;

namespace Application.Contracts.Requests.Genre;

public class CreateGenreRequest : BaseRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}