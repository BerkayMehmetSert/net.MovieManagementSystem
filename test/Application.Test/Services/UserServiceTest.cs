using System.Security.Claims;
using Application.Contracts.Constants.User;
using Application.Contracts.Requests.User;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using Application.Contracts.Validations.User;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class UserServiceTest : UserMockRepository
{
    private readonly UserService _userService;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock = new();
    private readonly UserLoginRequestValidation _userLoginRequestValidation;
    private readonly UserRegisterRequestValidator _userRegisterRequestValidator;
    private readonly AdminLoginRequestValidator _adminLoginRequestValidator;
    private readonly AdminRegisterRequestValidator _adminRegisterRequestValidator;
    private readonly ChangePasswordRequestValidation _changePasswordRequestValidator;

    public UserServiceTest(
        UserFakeData fakeData,
        UserLoginRequestValidation userLoginRequestValidation,
        UserRegisterRequestValidator userRegisterRequestValidator,
        AdminLoginRequestValidator adminLoginRequestValidator,
        AdminRegisterRequestValidator adminRegisterRequestValidator,
        ChangePasswordRequestValidation changePasswordRequestValidator
    ) :
        base(fakeData)
    {
        _userLoginRequestValidation = userLoginRequestValidation;
        _userRegisterRequestValidator = userRegisterRequestValidator;
        _adminLoginRequestValidator = adminLoginRequestValidator;
        _adminRegisterRequestValidator = adminRegisterRequestValidator;
        _changePasswordRequestValidator = changePasswordRequestValidator;
        _tokenServiceMock.Setup(x => x.CreateToken(It.IsAny<User>()))
            .Returns(new TokenResponse() { AccessToken = "1" });
        _contextAccessorMock.Setup(x => x.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, "11111111-1111-1111-1111-111111111111"));
        _userService = new UserService(
            MockRepository.Object,
            _tokenServiceMock.Object,
            UnitOfWork.Object,
            Mapper,
            _contextAccessorMock.Object
        );
    }

    [Fact]
    public void UserLoginValidRequestShouldReturnSuccess()
    {
        var request = new UserLoginRequest { Email = "user@system.com", Password = "1234" };
        var result = _userService.UserLogin(request);
        Assert.NotNull(result);
        Assert.Equal("1", result.AccessToken);
    }

    [Fact]
    public void UserLoginShouldValidRequestThrowNotFoundException()
    {
        var request = new UserLoginRequest { Email = "system@system.com", Password = "1234" };
        var exception = Assert.Throws<NotFoundException>(() => _userService.UserLogin(request));
        Assert.Equal(UserBusinessMessages.UserNotFoundByEmail, exception.Message);
    }

    [Fact]
    public void UserLoginShouldValidRequestThrowInValidPasswordException()
    {
        var request = new UserLoginRequest { Email = "user@system.com", Password = "123456" };
        var exception = Assert.Throws<BusinessException>(() => _userService.UserLogin(request));
        Assert.Equal(UserBusinessMessages.InvalidPassword, exception.Message);
    }

    [Fact]
    public void UserLoginShouldInValidRequestThrowValidationException()
    {
        var request = new UserLoginRequest() { Email = "", Password = "1234" };
        var result = _userLoginRequestValidation
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("Email"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.EmailRequired, result);
    }

    [Fact]
    public void AdminLoginValidRequestShouldReturnSuccess()
    {
        var request = new AdminLoginRequest { Email = "admin@system.com", Password = "1234" };
        var result = _userService.AdminLogin(request);
        Assert.NotNull(result);
        Assert.Equal("1", result.AccessToken);
    }

    [Fact]
    public void AdminLoginShouldValidRequestThrowNotFoundException()
    {
        var request = new AdminLoginRequest { Email = "admin2@system.com", Password = "1234" };
        var exception = Assert.Throws<NotFoundException>(() => _userService.AdminLogin(request));
        Assert.Equal(UserBusinessMessages.UserNotFoundByEmail, exception.Message);
    }

    [Fact]
    public void AdminLoginShouldValidRequestThrowInValidPasswordException()
    {
        var request = new AdminLoginRequest { Email = "admin@system.com", Password = "123456" };
        var exception = Assert.Throws<BusinessException>(() => _userService.AdminLogin(request));
        Assert.Equal(UserBusinessMessages.InvalidPassword, exception.Message);
    }

    [Fact]
    public void AdminLoginShouldInValidRequestThrowValidationException()
    {
        var request = new AdminLoginRequest { Email = "", Password = "1234" };
        var result = _adminLoginRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("Email"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.EmailRequired, result);
    }

    [Fact]
    public void UserRegisterValidRequestShouldReturnSuccess()
    {
        var request = new UserRegisterRequest { Email = "system@system.com", Password = "1234" };
        _userService.UserRegister(request);
        MockRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void UserRegisterValidRequestShouldThrowAlreadyExistsException()
    {
        var request = new UserRegisterRequest { Email = "admin@system.com", Password = "1234" };
        var exception = Assert.Throws<NotFoundException>(() => _userService.UserRegister(request));
        Assert.Equal(UserBusinessMessages.UserAlreadyExistByEmail, exception.Message);
    }

    [Fact]
    public void UserRegisterShouldInValidRequestThrowValidationException()
    {
        var request = new UserRegisterRequest { Email = "", Password = "1234" };
        var result = _userRegisterRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("Email"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.EmailRequired, result);
    }

    [Fact]
    public void AdminRegisterValidRequestShouldReturnSuccess()
    {
        var request = new AdminRegisterRequest { Email = "system@system.com", Password = "1234" };
        _userService.AdminRegister(request);
        MockRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void AdminRegisterValidRequestShouldThrowAlreadyExistsException()
    {
        var request = new AdminRegisterRequest { Email = "admin@system.com", Password = "1234" };
        var exception = Assert.Throws<NotFoundException>(() => _userService.AdminRegister(request));
        Assert.Equal(UserBusinessMessages.UserAlreadyExistByEmail, exception.Message);
    }

    [Fact]
    public void AdminRegisterInValidRequestShouldThrowValidationExistsException()
    {
        var request = new AdminRegisterRequest { Email = "", Password = "1234" };
        var result = _adminRegisterRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("Email"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.EmailRequired, result);
    }

    [Fact]
    public void ChangePasswordValidRequestShouldReturnSuccess()
    {
        var request = new ChangePasswordRequest
        {
            OldPassword = "1234",
            NewPassword = "12345",
            ConfirmPassword = "12345"
        };
        _userService.ChangePassword(request);
        MockRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void ChangePasswordValidRequestShouldThrowPasswordNotMatchWithOldPassword()
    {
        var request = new ChangePasswordRequest
        {
            OldPassword = "12345",
            NewPassword = "12345",
            ConfirmPassword = "12345"
        };
        var exception = Assert.Throws<BusinessException>(() => _userService.ChangePassword(request));
        Assert.Equal(UserBusinessMessages.PasswordNotMatchWithOldPassword, exception.Message);
    }

    [Fact]
    public void ChangePasswordValidRequestShouldThrowPasswordNotMatchWithConfirmPassword()
    {
        var request = new ChangePasswordRequest
        {
            OldPassword = "1234",
            NewPassword = "12345",
            ConfirmPassword = "123456"
        };
        var exception = Assert.Throws<BusinessException>(() => _userService.ChangePassword(request));
        Assert.Equal(UserBusinessMessages.PasswordNotMatchWithConfirmPassword, exception.Message);
    }

    [Fact]
    public void ChangePasswordInValidRequestShouldThrowValidationException()
    {
        var request = new ChangePasswordRequest { OldPassword = "", };
        var result = _changePasswordRequestValidator
            .Validate(request)
            .Errors
            .Where(x => x.PropertyName.Equals("OldPassword"))
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(request);
        Assert.Equal(UserValidationMessages.PasswordRequired, result);
    }

    [Fact]
    public void DeleteUserShouldReturnSuccess()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        _userService.DeleteUser(userId);
        MockRepository.Verify(x => x.Delete(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void DeleteUserShouldThrowNotFoundException()
    {
        var userId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _userService.DeleteUser(userId));
        Assert.Equal(UserBusinessMessages.UserNotFoundById, exception.Message);
    }

    [Fact]
    public void GetUserByIdShouldReturnSuccess()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _userService.GetUserById(userId);
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public void GetUserByIdShouldThrowNotFoundException()
    {
        var userId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _userService.GetUserById(userId));
        Assert.Equal(UserBusinessMessages.UserNotFoundById, exception.Message);
    }

    [Fact]
    public void GetAllUsersShouldReturnSuccess()
    {
        var result = _userService.GetAllUsers();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}