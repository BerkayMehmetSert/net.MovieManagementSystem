using Application.Contracts.Requests.Actor;
using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mappers;

public class ActorMapper : Profile
{
    public ActorMapper()
    {
        CreateMap<CreateActorRequest, Actor>();
        CreateMap<UpdateActorRequest, Actor>();
        CreateMap<Actor, ActorResponse>()
            .ForMember(dest => dest.Awards,
                opt => opt.MapFrom(src => src.ActorAwards.Select(ur => ur.Award)));
        CreateMap<Award, AwardResponse>();
    }
}