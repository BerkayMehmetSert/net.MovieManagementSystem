using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class AwardFakeData : BaseFakeData<Award>
{
    public override List<Award> CreateFakeData()
    {
        return new List<Award>
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Award 1",
                Date = new DateTime(2021, 1, 1),
            },
            new()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Award 2",
                Date = new DateTime(2021, 1, 2),
                ActorAwards = new List<ActorAward>
                {
                    new()
                    {
                        ActorId = new Guid("11111111-1111-1111-1111-111111111111"),
                        AwardId = new Guid("22222222-2222-2222-2222-222222222222")
                    }
                }
            }
        };
    }
}