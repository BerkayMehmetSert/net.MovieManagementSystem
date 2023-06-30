using Core.Application.Request;

namespace Application.Contracts.Requests.Rating;

public class CreateRatingRequest : BaseRequest
{
    public decimal Score { get; set; }
    public string? Description { get; set; }
}