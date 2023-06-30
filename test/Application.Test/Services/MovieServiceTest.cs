using Application.Contracts.Constants.Actor;
using Application.Contracts.Constants.Cinema;
using Application.Contracts.Constants.Director;
using Application.Contracts.Constants.Genre;
using Application.Contracts.Constants.Language;
using Application.Contracts.Constants.Movie;
using Application.Contracts.Constants.Rating;
using Application.Contracts.Requests.Movie;
using Application.Contracts.Services;
using Application.Contracts.Validations.Movie;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class MovieServiceTest : MovieMockRepository
{
    private readonly Mock<IActorService> _actorService = new();
    private readonly Mock<ICinemaService> _cinemaService = new();
    private readonly Mock<IDirectorService> _directorService = new();
    private readonly Mock<IGenreService> _genreService = new();
    private readonly Mock<ILanguageService> _languageService = new();
    private readonly Mock<IRatingService> _ratingService = new();
    private readonly CreateMovieRequestValidator _createMovieRequestValidator;
    private readonly UpdateMovieRequestValidator _updateMovieRequestValidator;
    private readonly MovieService _service;

    public MovieServiceTest(
        MovieFakeData fakeData,
        CreateMovieRequestValidator createMovieRequestValidator,
        UpdateMovieRequestValidator updateMovieRequestValidator
    ) : base(fakeData)
    {
        _createMovieRequestValidator = createMovieRequestValidator;
        _updateMovieRequestValidator = updateMovieRequestValidator;
        _service = new MovieService(
            MockRepository.Object,
            Mapper,
            UnitOfWork.Object,
            _actorService.Object,
            _cinemaService.Object,
            _directorService.Object,
            _genreService.Object,
            _languageService.Object,
            _ratingService.Object
        );
    }

    [Fact]
    public void CreateMovieValidRequestShouldReturnSuccess()
    {
        var request = new CreateMovieRequest { Title = "Test Movie" };

        _service.CreateMovie(request);
        MockRepository.Verify(x => x.Add(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void CreateMovieValidRequestShouldThrowMovieAlreadyExistsException()
    {
        var request = new CreateMovieRequest { Title = "Movie 1" };
        var exception = Assert.Throws<BusinessException>(() => _service.CreateMovie(request));
        Assert.Equal(MovieBusinessMessages.MovieAlreadyExistsByName, exception.Message);
    }

    [Fact]
    public void CreateMovieInvalidRequestShouldThrowValidationException()
    {
        var request = new CreateMovieRequest { Title = "" };
        var result = _createMovieRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Title")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(MovieValidationMessages.TitleRequired, result);
    }

    [Fact]
    public void UpdateMovieValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new UpdateMovieRequest { Title = "Test Movie" };
        _service.UpdateMovie(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void UpdateMovieValidRequestShouldThrowMovieAlreadyExistsException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new UpdateMovieRequest { Title = "Movie 2" };
        var exception = Assert.Throws<BusinessException>(() => _service.UpdateMovie(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieAlreadyExistsByName, exception.Message);
    }

    [Fact]
    public void UpdateMovieValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var request = new UpdateMovieRequest { Title = "Test Movie" };
        var exception = Assert.Throws<NotFoundException>(() => _service.UpdateMovie(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateMovieInvalidRequestShouldThrowValidationException()
    {
        var request = new UpdateMovieRequest { Title = "" };
        var result = _updateMovieRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Title")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(MovieValidationMessages.TitleRequired, result);
    }

    [Fact]
    public void AddMovieActorValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieActorRequest { ActorIds = new List<Guid> { actorId } };
        _actorService.Setup(x => x.GetActorEntityById(It.IsAny<Guid>())).Returns(new Actor() { Id = actorId });
        _service.AddMovieActor(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void AddMovieActorValidRequestShouldThrowActorNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var actorId = Guid.Empty;
        var request = new AddMovieActorRequest { ActorIds = new List<Guid> { actorId } };
        _actorService.Setup(x => x.GetActorEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(ActorBusinessMessages.ActorNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieActor(movieId, request));
        Assert.Equal(ActorBusinessMessages.ActorNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieActorValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieActorRequest { ActorIds = new List<Guid> { actorId } };
        _actorService.Setup(x => x.GetActorEntityById(It.IsAny<Guid>())).Returns(new Actor() { Id = actorId });
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieActor(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieCinemaValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieCinemaRequest { CinemaIds = new List<Guid> { cinemaId } };
        _cinemaService.Setup(x => x.GetCinemaEntityById(It.IsAny<Guid>())).Returns(new Cinema { Id = cinemaId });
        _service.AddMovieCinema(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void AddMovieCinemaValidRequestShouldThrowCinemaNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var cinemaId = Guid.Empty;
        var request = new AddMovieCinemaRequest { CinemaIds = new List<Guid> { cinemaId } };
        _cinemaService.Setup(x => x.GetCinemaEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(CinemaBusinessMessages.CinemaNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieCinema(movieId, request));
        Assert.Equal(CinemaBusinessMessages.CinemaNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieCinemaValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieCinemaRequest { CinemaIds = new List<Guid> { cinemaId } };
        _cinemaService.Setup(x => x.GetCinemaEntityById(It.IsAny<Guid>())).Returns(new Cinema { Id = cinemaId });
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieCinema(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieDirectorValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var directorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieDirectorRequest { DirectorIds = new List<Guid> { directorId } };
        _directorService.Setup(x => x.GetDirectorEntityById(It.IsAny<Guid>()))
            .Returns(new Director { Id = directorId });
        _service.AddMovieDirector(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void AddMovieDirectorValidRequestShouldThrowDirectorNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var directorId = Guid.Empty;
        var request = new AddMovieDirectorRequest { DirectorIds = new List<Guid> { directorId } };
        _directorService.Setup(x => x.GetDirectorEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(DirectorBusinessMessages.DirectorNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieDirector(movieId, request));
        Assert.Equal(DirectorBusinessMessages.DirectorNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieDirectorValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var directorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieDirectorRequest { DirectorIds = new List<Guid> { directorId } };
        _directorService.Setup(x => x.GetDirectorEntityById(It.IsAny<Guid>()))
            .Returns(new Director { Id = directorId });
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieDirector(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieGenreValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieGenreRequest { GenreIds = new List<Guid> { genreId } };
        _genreService.Setup(x => x.GetGenreEntityById(It.IsAny<Guid>())).Returns(new Genre { Id = genreId });
        _service.AddMovieGenre(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void AddMovieGenreValidRequestShouldThrowGenreNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var genreId = Guid.Empty;
        var request = new AddMovieGenreRequest { GenreIds = new List<Guid> { genreId } };
        _genreService.Setup(x => x.GetGenreEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(GenreBusinessMessages.GenreNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieGenre(movieId, request));
        Assert.Equal(GenreBusinessMessages.GenreNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieGenreValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieGenreRequest { GenreIds = new List<Guid> { genreId } };
        _genreService.Setup(x => x.GetGenreEntityById(It.IsAny<Guid>())).Returns(new Genre { Id = genreId });
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieGenre(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieLanguageValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var languageId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieLanguageRequest { LanguageIds = new List<Guid> { languageId } };
        _languageService.Setup(x => x.GetLanguageEntityById(It.IsAny<Guid>()))
            .Returns(new Language { Id = languageId });
        _service.AddMovieLanguage(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void AddMovieLanguageValidRequestShouldThrowLanguageNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var languageId = Guid.Empty;
        var request = new AddMovieLanguageRequest { LanguageIds = new List<Guid> { languageId } };
        _languageService.Setup(x => x.GetLanguageEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(LanguageBusinessMessages.LanguageNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieLanguage(movieId, request));
        Assert.Equal(LanguageBusinessMessages.LanguageNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieLanguageValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var languageId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieLanguageRequest { LanguageIds = new List<Guid> { languageId } };
        _languageService.Setup(x => x.GetLanguageEntityById(It.IsAny<Guid>()))
            .Returns(new Language { Id = languageId });
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieLanguage(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieRatingValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var ratingId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieRatingRequest { RatingIds = new List<Guid> { ratingId } };
        _ratingService.Setup(x => x.GetRatingEntityById(It.IsAny<Guid>())).Returns(new Rating { Id = ratingId });
        _service.AddMovieRating(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void AddMovieRatingValidRequestShouldThrowRatingNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var ratingId = Guid.Empty;
        var request = new AddMovieRatingRequest { RatingIds = new List<Guid> { ratingId } };
        _ratingService.Setup(x => x.GetRatingEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(RatingBusinessMessages.RatingNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieRating(movieId, request));
        Assert.Equal(RatingBusinessMessages.RatingNotFoundById, exception.Message);
    }

    [Fact]
    public void AddMovieRatingValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var ratingId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddMovieRatingRequest { RatingIds = new List<Guid> { ratingId } };
        _ratingService.Setup(x => x.GetRatingEntityById(It.IsAny<Guid>())).Returns(new Rating { Id = ratingId });
        var exception = Assert.Throws<NotFoundException>(() => _service.AddMovieRating(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void DeleteMovieValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.DeleteMovie(movieId);
        MockRepository.Verify(x => x.Delete(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void DeleteMovieValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.DeleteMovie(movieId));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieActorValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieActorRequest { ActorIds = new List<Guid> { actorId } };
        _actorService.Setup(x => x.GetActorEntityById(It.IsAny<Guid>())).Returns(new Actor { Id = actorId });
        _service.RemoveMovieActor(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void RemoveMovieActorValidRequestShouldThrowActorNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var actorId = Guid.Empty;
        var request = new RemoveMovieActorRequest { ActorIds = new List<Guid> { actorId } };
        _actorService.Setup(x => x.GetActorEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(ActorBusinessMessages.ActorNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieActor(movieId, request));
        Assert.Equal(ActorBusinessMessages.ActorNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieActorValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieActorRequest { ActorIds = new List<Guid> { actorId } };
        _actorService.Setup(x => x.GetActorEntityById(It.IsAny<Guid>())).Returns(new Actor { Id = actorId });
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieActor(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieCinemaValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieCinemaRequest { CinemaIds = new List<Guid> { cinemaId } };
        _cinemaService.Setup(x => x.GetCinemaEntityById(It.IsAny<Guid>())).Returns(new Cinema { Id = cinemaId });
        _service.RemoveMovieCinema(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void RemoveMovieCinemaValidRequestShouldThrowCinemaNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var cinemaId = Guid.Empty;
        var request = new RemoveMovieCinemaRequest { CinemaIds = new List<Guid> { cinemaId } };
        _cinemaService.Setup(x => x.GetCinemaEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(CinemaBusinessMessages.CinemaNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieCinema(movieId, request));
        Assert.Equal(CinemaBusinessMessages.CinemaNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieCinemaValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieCinemaRequest { CinemaIds = new List<Guid> { cinemaId } };
        _cinemaService.Setup(x => x.GetCinemaEntityById(It.IsAny<Guid>())).Returns(new Cinema { Id = cinemaId });
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieCinema(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieDirectorValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var directorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieDirectorRequest { DirectorIds = new List<Guid> { directorId } };
        _directorService.Setup(x => x.GetDirectorEntityById(It.IsAny<Guid>()))
            .Returns(new Director { Id = directorId });
        _service.RemoveMovieDirector(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void RemoveMovieDirectorValidRequestShouldThrowDirectorNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var directorId = Guid.Empty;
        var request = new RemoveMovieDirectorRequest { DirectorIds = new List<Guid> { directorId } };
        _directorService.Setup(x => x.GetDirectorEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(DirectorBusinessMessages.DirectorNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieDirector(movieId, request));
        Assert.Equal(DirectorBusinessMessages.DirectorNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieDirectorValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var directorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieDirectorRequest { DirectorIds = new List<Guid> { directorId } };
        _directorService.Setup(x => x.GetDirectorEntityById(It.IsAny<Guid>()))
            .Returns(new Director { Id = directorId });
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieDirector(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieGenreValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieGenreRequest { GenreIds = new List<Guid> { genreId } };
        _genreService.Setup(x => x.GetGenreEntityById(It.IsAny<Guid>())).Returns(new Genre { Id = genreId });
        _service.RemoveMovieGenre(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void RemoveMovieGenreValidRequestShouldThrowGenreNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var genreId = Guid.Empty;
        var request = new RemoveMovieGenreRequest { GenreIds = new List<Guid> { genreId } };
        _genreService.Setup(x => x.GetGenreEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(GenreBusinessMessages.GenreNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieGenre(movieId, request));
        Assert.Equal(GenreBusinessMessages.GenreNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieGenreValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieGenreRequest { GenreIds = new List<Guid> { genreId } };
        _genreService.Setup(x => x.GetGenreEntityById(It.IsAny<Guid>())).Returns(new Genre { Id = genreId });
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieGenre(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieLanguageValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var languageId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieLanguageRequest { LanguageIds = new List<Guid> { languageId } };
        _languageService.Setup(x => x.GetLanguageEntityById(It.IsAny<Guid>()))
            .Returns(new Language { Id = languageId });
        _service.RemoveMovieLanguage(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void RemoveMovieLanguageValidRequestShouldThrowLanguageNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var languageId = Guid.Empty;
        var request = new RemoveMovieLanguageRequest { LanguageIds = new List<Guid> { languageId } };
        _languageService.Setup(x => x.GetLanguageEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(LanguageBusinessMessages.LanguageNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieLanguage(movieId, request));
        Assert.Equal(LanguageBusinessMessages.LanguageNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieLanguageValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var languageId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieLanguageRequest { LanguageIds = new List<Guid> { languageId } };
        _languageService.Setup(x => x.GetLanguageEntityById(It.IsAny<Guid>()))
            .Returns(new Language { Id = languageId });
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieLanguage(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieRatingValidRequestShouldReturnSuccess()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var ratingId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieRatingRequest { RatingIds = new List<Guid> { ratingId } };
        _ratingService.Setup(x => x.GetRatingEntityById(It.IsAny<Guid>())).Returns(new Rating { Id = ratingId });
        _service.RemoveMovieRating(movieId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public void RemoveMovieRatingValidRequestShouldThrowRatingNotFoundException()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var ratingId = Guid.Empty;
        var request = new RemoveMovieRatingRequest { RatingIds = new List<Guid> { ratingId } };
        _ratingService.Setup(x => x.GetRatingEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(RatingBusinessMessages.RatingNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieRating(movieId, request));
        Assert.Equal(RatingBusinessMessages.RatingNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveMovieRatingValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var ratingId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveMovieRatingRequest { RatingIds = new List<Guid> { ratingId } };
        _ratingService.Setup(x => x.GetRatingEntityById(It.IsAny<Guid>())).Returns(new Rating { Id = ratingId });
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveMovieRating(movieId, request));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void GetMovieByIdValidRequestShouldReturnMovie()
    {
        var movieId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetMovieById(movieId);
        Assert.Equal(movieId, result.Id);
    }

    [Fact]
    public void GetMovieByIdValidRequestShouldThrowMovieNotFoundException()
    {
        var movieId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetMovieById(movieId));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundById, exception.Message);
    }

    [Fact]
    public void GetMovieByTitleValidRequestShouldReturnMovie()
    {
        const string title = "Movie 1";
        var result = _service.GetMovieByTitle(title);
        Assert.Equal(title, result.Title);
    }

    [Fact]
    public void GetMovieByTitleValidRequestShouldThrowMovieNotFoundException()
    {
        const string title = "Test Movie";
        var exception = Assert.Throws<NotFoundException>(() => _service.GetMovieByTitle(title));
        Assert.Equal(MovieBusinessMessages.MovieNotFoundByTitle, exception.Message);
    }

    [Fact]
    public void GetAllMoviesValidRequestShouldReturnMovies()
    {
        var result = _service.GetAllMovies();
        Assert.Equal(2, result.Count);
    }
}