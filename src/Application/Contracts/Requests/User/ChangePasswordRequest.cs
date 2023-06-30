using Core.Application.Request;

namespace Application.Contracts.Requests.User;

public class ChangePasswordRequest : BaseRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}