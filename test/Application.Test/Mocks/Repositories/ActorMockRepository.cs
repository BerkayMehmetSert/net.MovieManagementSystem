using Application.Contracts.Mappers;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class ActorMockRepository : BaseMockRepository<IActorRepository, Actor, ActorMapper, ActorFakeData>
{
    public ActorMockRepository(ActorFakeData fakeData) : base(fakeData)
    {
    }
}