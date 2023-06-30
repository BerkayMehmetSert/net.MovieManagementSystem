using Core.Domain;

namespace Application.Contracts.Requests.Language;

public class UpdateLanguageRequest : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}