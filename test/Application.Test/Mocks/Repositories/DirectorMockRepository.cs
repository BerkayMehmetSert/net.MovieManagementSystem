using Application.Contracts.Mappers;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class DirectorMockRepository : BaseMockRepository<IDirectorRepository, Director, DirectorMapper, DirectorFakeData>
{
    public DirectorMockRepository(DirectorFakeData fakeData) : base(fakeData)
    {
    }
}