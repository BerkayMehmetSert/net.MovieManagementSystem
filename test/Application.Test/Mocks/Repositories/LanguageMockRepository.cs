using Application.Contracts.Mappers;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class LanguageMockRepository : BaseMockRepository<ILanguageRepository, Language, LanguageMapper, LanguageFakeData>
{
    public LanguageMockRepository(LanguageFakeData fakeData) : base(fakeData)
    {
    }
}