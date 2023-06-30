using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class DirectorFakeData : BaseFakeData<Director>
{
    public override List<Director> CreateFakeData()
    {
        return new List<Director>
        {
            new ()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111")
            }
        };
    }
}