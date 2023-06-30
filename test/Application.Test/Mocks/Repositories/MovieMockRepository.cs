using Application.Contracts.Mappers;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class MovieMockRepository : BaseMockRepository<IMovieRepository, Movie, MovieMapper, MovieFakeData>
{
    public MovieMockRepository(MovieFakeData fakeData) : base(fakeData)
    {
    }
}