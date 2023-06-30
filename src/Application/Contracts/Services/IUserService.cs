using Application.Contracts.Requests.User;
using Application.Contracts.Responses;

namespace Application.Contracts.Services;

public interface IUserService
{
    TokenResponse UserLogin(UserLoginRequest request);
    TokenResponse AdminLogin(AdminLoginRequest request);
    void UserRegister(UserRegisterRequest request);
    void AdminRegister(AdminRegisterRequest request);
    void ChangePassword(ChangePasswordRequest request);
    void DeleteUser(Guid id);
    UserResponse GetUserById(Guid id);
    List<UserResponse> GetAllUsers();
}