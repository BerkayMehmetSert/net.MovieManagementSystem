using Core.Application.Request;

namespace Application.Contracts.Requests.Actor;

public class RemoveActorAwardRequest : BaseRequest
{
    public ICollection<Guid> AwardIds { get; set; }
}