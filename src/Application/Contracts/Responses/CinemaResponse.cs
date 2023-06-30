using Core.Application.Response;

namespace Application.Contracts.Responses;

public class CinemaResponse : BaseResponse
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}