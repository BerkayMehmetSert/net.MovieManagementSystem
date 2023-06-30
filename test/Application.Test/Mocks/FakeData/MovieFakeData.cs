using Core.Test.FakeData;
using Domain.Entities;

namespace Application.Test.Mocks.FakeData;

public class MovieFakeData : BaseFakeData<Movie>
{
    public override List<Movie> CreateFakeData()
    {
        return new List<Movie>
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Title = "Movie 1",
                MovieActors = new List<MovieActor>
                {
                    new()
                    {
                        ActorId = new Guid("11111111-1111-1111-1111-111111111111"),
                        MovieId = new Guid("11111111-1111-1111-1111-111111111111")
                    }
                },
                MovieDirectors = new List<MovieDirector>
                {
                    new()
                    {
                        DirectorId = new Guid("11111111-1111-1111-1111-111111111111"),
                        MovieId = new Guid("11111111-1111-1111-1111-111111111111")
                    }
                },
                MovieGenres = new List<MovieGenre>
                {
                    new()
                    {
                        GenreId = new Guid("11111111-1111-1111-1111-111111111111"),
                        MovieId = new Guid("11111111-1111-1111-1111-111111111111")
                    }
                },
                MovieCinemas = new List<MovieCinema>
                {
                    new()
                    {
                        CinemaId = new Guid("11111111-1111-1111-1111-111111111111"),
                        MovieId = new Guid("11111111-1111-1111-1111-111111111111")
                    }
                },
                MovieLanguages = new List<MovieLanguage>
                {
                    new()
                    {
                        LanguageId = new Guid("11111111-1111-1111-1111-111111111111"),
                        MovieId = new Guid("11111111-1111-1111-1111-111111111111")
                    }
                },
                MovieRatings = new List<MovieRating>
                {
                    new()
                    {
                        RatingId = new Guid("11111111-1111-1111-1111-111111111111"),
                        MovieId = new Guid("11111111-1111-1111-1111-111111111111")
                    }
                }
            },
            new()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Title = "Movie 2",
            }
        };
    }
}