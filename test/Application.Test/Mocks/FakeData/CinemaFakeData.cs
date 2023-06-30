using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class CinemaFakeData : BaseFakeData<Cinema>
{
    public override List<Cinema> CreateFakeData()
    {
        return new List<Cinema>
        {
            new ()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Cinema 1",
                Address = "Address 1"
            },
            new ()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Cinema 2",
                MovieCinemas = new List<MovieCinema>
                {
                    new ()
                    {
                        MovieId = new Guid("44444444-4444-4444-4444-444444444444"),
                        CinemaId = new Guid("22222222-2222-2222-2222-222222222222")
                    }
                }
            },
        };
    }
}