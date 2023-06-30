using Application.Contracts.Mappers;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class CinemaMockRepository : BaseMockRepository<ICinemaRepository, Cinema, CinemaMapper, CinemaFakeData>
{
    public CinemaMockRepository(CinemaFakeData fakeData) : base(fakeData)
    {
    }
}