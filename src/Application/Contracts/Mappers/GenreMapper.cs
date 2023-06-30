using Application.Contracts.Requests.Genre;
using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mappers;

public class GenreMapper : Profile
{
    public GenreMapper()
    {
        CreateMap<CreateGenreRequest, Genre>();
        CreateMap<UpdateGenreRequest, Genre>();
        CreateMap<Genre, GenreResponse>();
    }
}