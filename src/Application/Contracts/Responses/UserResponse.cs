using Core.Application.Response;

namespace Application.Contracts.Responses;

public class UserResponse : BaseResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}