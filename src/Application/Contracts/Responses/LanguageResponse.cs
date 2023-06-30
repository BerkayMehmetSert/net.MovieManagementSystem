using Core.Application.Response;

namespace Application.Contracts.Responses;

public class LanguageResponse : BaseResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
}