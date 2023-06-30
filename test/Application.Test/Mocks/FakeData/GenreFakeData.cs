using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class GenreFakeData : BaseFakeData<Genre>
{
    public override List<Genre> CreateFakeData()
    {
        return new List<Genre>
        {
            new ()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Genre 1"
            },
            new ()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Genre 2"
            }
        };
    }
}