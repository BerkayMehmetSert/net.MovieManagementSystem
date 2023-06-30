using Core.Application.Request;

namespace Application.Contracts.Requests.Language;

public class CreateLanguageRequest : BaseRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}