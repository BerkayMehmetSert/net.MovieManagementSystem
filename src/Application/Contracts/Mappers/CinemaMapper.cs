using Application.Contracts.Requests.Cinema;
using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mappers;

public class CinemaMapper : Profile
{
    public CinemaMapper()
    {
        CreateMap<CreateCinemaRequest, Cinema>();
        CreateMap<UpdateCinemaAddressRequest, Cinema>();
        CreateMap<UpdateCinemaNameRequest, Cinema>();
        CreateMap<Cinema, CinemaResponse>();
    }
}