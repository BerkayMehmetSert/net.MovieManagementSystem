using Core.Application.Request;

namespace Application.Contracts.Requests.Actor;

public class AddActorAwardRequest : BaseRequest
{
    public ICollection<Guid> AwardIds { get; set; }
}