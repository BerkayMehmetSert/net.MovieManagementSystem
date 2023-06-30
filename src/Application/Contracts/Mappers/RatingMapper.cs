using Application.Contracts.Requests.Rating;
using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mappers;

public class RatingMapper : Profile
{
    public RatingMapper()
    {
        CreateMap<CreateRatingRequest, Rating>();
        CreateMap<UpdateRatingRequest, Rating>();
        CreateMap<Rating, RatingResponse>();
    }
}