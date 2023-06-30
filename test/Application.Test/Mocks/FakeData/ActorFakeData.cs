using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class ActorFakeData : BaseFakeData<Actor>
{
    public override List<Actor> CreateFakeData()
    {
        return new List<Actor>
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                ActorAwards = new List<ActorAward>
                {
                    new ()
                    {
                        ActorId = new Guid("11111111-1111-1111-1111-111111111111"),
                        AwardId = new Guid("22222222-2222-2222-2222-222222222222"),
                        Award = new Award
                        {
                            Id = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "Award 1"
                        }
                    }
                }
            }
        };
    }
}