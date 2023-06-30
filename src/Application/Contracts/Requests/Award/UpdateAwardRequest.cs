using Core.Application.Request;

namespace Application.Contracts.Requests.Award;

public class UpdateAwardRequest : BaseRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}