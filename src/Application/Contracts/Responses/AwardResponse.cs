using Core.Application.Response;

namespace Application.Contracts.Responses;

public class AwardResponse : BaseResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}