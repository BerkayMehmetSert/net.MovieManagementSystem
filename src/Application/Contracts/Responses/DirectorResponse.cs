using Core.Application.Response;

namespace Application.Contracts.Responses;

public class DirectorResponse : BaseResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
}