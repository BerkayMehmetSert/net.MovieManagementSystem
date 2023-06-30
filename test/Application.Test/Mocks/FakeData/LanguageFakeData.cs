using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class LanguageFakeData : BaseFakeData<Language>
{
    public override List<Language> CreateFakeData()
    {
        return new List<Language>
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Language 1"
            },
            new()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Language 2"
            },
        };
    }
}