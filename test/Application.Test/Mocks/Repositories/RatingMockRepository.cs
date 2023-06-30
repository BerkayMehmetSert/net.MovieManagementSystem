using Application.Contracts.Mappers;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class RatingMockRepository : BaseMockRepository<IRatingRepository, Rating, RatingMapper, RatingFakeData>
{
    public RatingMockRepository(RatingFakeData fakeData) : base(fakeData)
    {
    }
}