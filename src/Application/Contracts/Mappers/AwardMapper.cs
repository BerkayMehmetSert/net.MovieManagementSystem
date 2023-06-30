using Application.Contracts.Requests.Award;
using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mappers;

public class AwardMapper : Profile
{
    public AwardMapper()
    {
        CreateMap<CreateAwardRequest, Award>();
        CreateMap<UpdateAwardRequest, Award>();
        CreateMap<Award, AwardResponse>();
    }
}