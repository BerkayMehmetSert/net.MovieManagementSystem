using Application.Contracts.Requests.User;
using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<AdminLoginRequest, User>();
        CreateMap<UserLoginRequest, User>();
        CreateMap<AdminRegisterRequest, User>();
        CreateMap<UserRegisterRequest, User>();
        CreateMap<User, UserResponse>();
    }
}