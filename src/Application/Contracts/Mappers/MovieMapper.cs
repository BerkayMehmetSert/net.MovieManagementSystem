using Application.Contracts.Requests.Movie;
using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mappers;

public class MovieMapper : Profile
{
    public MovieMapper()
    {
        CreateMap<CreateMovieRequest, Movie>();
        CreateMap<UpdateMovieRequest, Movie>();
        CreateMap<Movie, MovieResponse>()
            .ForMember(dest => dest.Actors,
                opt => opt.MapFrom(src => src.MovieActors.Select(ur => ur.Actor)))
            .ForMember(dest => dest.Cinemas,
                opt => opt.MapFrom(src => src.MovieCinemas.Select(ur => ur.Cinema)))
            .ForMember(dest => dest.Directors,
                opt => opt.MapFrom(src => src.MovieDirectors.Select(ur => ur.Director)))
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.MovieGenres.Select(ur => ur.Genre)))
            .ForMember(dest => dest.Languages,
                opt => opt.MapFrom(src => src.MovieLanguages.Select(ur => ur.Language)))
            .ForMember(dest => dest.Ratings,
                opt => opt.MapFrom(src => src.MovieRatings.Select(ur => ur.Rating)));
    }
}