using Application.Contracts.Mappers;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class GenreMockRepository : BaseMockRepository<IGenreRepository, Genre, GenreMapper, GenreFakeData>
{
    public GenreMockRepository(GenreFakeData fakeData) : base(fakeData)
    {
    }
}