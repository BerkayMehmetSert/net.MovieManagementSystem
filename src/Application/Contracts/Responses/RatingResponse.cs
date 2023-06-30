using Core.Application.Response;

namespace Application.Contracts.Responses;

public class RatingResponse : BaseResponse
{
    public decimal Score { get; set; }
    public string? Description { get; set; }
}