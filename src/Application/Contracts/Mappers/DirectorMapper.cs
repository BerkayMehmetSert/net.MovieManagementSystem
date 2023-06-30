using Application.Contracts.Requests.Director;
using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mappers;

public class DirectorMapper : Profile
{
    public DirectorMapper()
    {
        CreateMap<CreateDirectorRequest, Director>();
        CreateMap<UpdateDirectorRequest, Director>();
        CreateMap<Director, DirectorResponse>();
    }
}