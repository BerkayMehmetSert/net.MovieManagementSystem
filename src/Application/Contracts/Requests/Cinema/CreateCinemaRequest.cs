using Core.Application.Request;

namespace Application.Contracts.Requests.Cinema;

public class CreateCinemaRequest : BaseRequest
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}