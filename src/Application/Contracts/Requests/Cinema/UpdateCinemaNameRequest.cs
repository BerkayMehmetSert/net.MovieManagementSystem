using Core.Application.Request;

namespace Application.Contracts.Requests.Cinema;

public class UpdateCinemaNameRequest : BaseRequest
{
    public string Name { get; set; }
}