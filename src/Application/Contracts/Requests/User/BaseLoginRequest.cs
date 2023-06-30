using Core.Application.Request;

namespace Application.Contracts.Requests.User;

public class BaseLoginRequest : BaseRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}