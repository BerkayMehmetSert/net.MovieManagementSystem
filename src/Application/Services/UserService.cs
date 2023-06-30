using System.Security.Claims;
using Application.Contracts.Constants.User;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.User;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence;
using Core.Utilities.Security;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHttpContextAccessor contextAccessor)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
    }

    public TokenResponse UserLogin(UserLoginRequest request) => LoginUserInternal(request);

    public TokenResponse AdminLogin(AdminLoginRequest request) => LoginUserInternal(request);

    public void UserRegister(UserRegisterRequest request) => RegisterUserInternal(request, "User");

    public void AdminRegister(AdminRegisterRequest request) => RegisterUserInternal(request, "Admin");

    public void ChangePassword(ChangePasswordRequest request)
    {
        var user = GetUserEntity();
        if (!HashingHelper.VerifyPasswordHash(request.OldPassword, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(UserBusinessMessages.PasswordNotMatchWithOldPassword);
        if (request.NewPassword != request.ConfirmPassword)
            throw new BusinessException(UserBusinessMessages.PasswordNotMatchWithConfirmPassword);

        HashingHelper.CreatePasswordHash(request.NewPassword, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _userRepository.Update(user);
        _unitOfWork.SaveChanges();
    }

    public void DeleteUser(Guid id)
    {
        var user = GetUserEntity(id);
        _userRepository.Delete(user);
        _unitOfWork.SaveChanges();
    }

    public UserResponse GetUserById(Guid id)
    {
        var user = GetUserEntity(id);
        return _mapper.Map<UserResponse>(user);
    }

    public List<UserResponse> GetAllUsers()
    {
        var users = _userRepository.GetAll();
        return _mapper.Map<List<UserResponse>>(users);
    }

    private void RegisterUserInternal<T>(T request, string role) where T : BaseRegisterRequest
    {
        CheckIfUserExistByEmail(request.Email);
        HashingHelper.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
        var user = _mapper.Map<User>(request);
        user.Role = role;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _userRepository.Add(user);
        _unitOfWork.SaveChanges();
    }

    private TokenResponse LoginUserInternal<T>(T request) where T : BaseLoginRequest
    {
        var user = _userRepository.Get(x => x.Email.Equals(request.Email));
        if (user is null)
            throw new NotFoundException(UserBusinessMessages.UserNotFoundByEmail);
        if (!HashingHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(UserBusinessMessages.InvalidPassword);

        return _tokenService.CreateToken(user);
    }

    private void CheckIfUserExistByEmail(string email)
    {
        var user = _userRepository.Get(x => x.Email.Equals(email));
        if (user is not null)
            throw new NotFoundException(UserBusinessMessages.UserAlreadyExistByEmail);
    }

    private User GetUserEntity(Guid? id = null)
    {
        var userId = id ?? GetUserIdFromToken();
        var user = _userRepository.Get(x => x.Id.Equals(userId));
        if (user is null)
            throw new NotFoundException(UserBusinessMessages.UserNotFoundById);
        return user;
    }

    private Guid GetUserIdFromToken()
    {
        var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new NotFoundException(UserBusinessMessages.UserNotFoundById);
        return Guid.Parse(userId);
    }
}