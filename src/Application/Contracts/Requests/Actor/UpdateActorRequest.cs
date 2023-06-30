using Core.Application.Request;

namespace Application.Contracts.Requests.Actor;

public class UpdateActorRequest : BaseRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
}