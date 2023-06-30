using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class RatingFakeData : BaseFakeData<Rating>
{
    public override List<Rating> CreateFakeData()
    {
        return new List<Rating>
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111")
            }
        };
    }
}