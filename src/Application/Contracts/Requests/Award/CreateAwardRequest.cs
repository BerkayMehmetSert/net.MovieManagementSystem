using Core.Application.Request;

namespace Application.Contracts.Requests.Award;

public class CreateAwardRequest : BaseRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}