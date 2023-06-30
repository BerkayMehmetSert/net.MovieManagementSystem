using Application.Contracts.Mappers;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class AwardMockRepository : BaseMockRepository<IAwardRepository, Award, AwardMapper, AwardFakeData>
{
    public AwardMockRepository(AwardFakeData fakeData) : base(fakeData)
    {
    }
}