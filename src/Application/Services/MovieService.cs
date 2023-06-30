using Application.Contracts.Constants.Movie;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Movie;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActorService _actorService;
    private readonly ICinemaService _cinemaService;
    private readonly IDirectorService _directorService;
    private readonly IGenreService _genreService;
    private readonly ILanguageService _languageService;
    private readonly IRatingService _ratingService;

    public MovieService(
        IMovieRepository movieRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IActorService actorService,
        ICinemaService cinemaService,
        IDirectorService directorService,
        IGenreService genreService,
        ILanguageService languageService,
        IRatingService ratingService
    )
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _actorService = actorService;
        _cinemaService = cinemaService;
        _directorService = directorService;
        _genreService = genreService;
        _languageService = languageService;
        _ratingService = ratingService;
    }

    public void CreateMovie(CreateMovieRequest request)
    {
        CheckIfMovieExistsByTitle(request.Title);
        var movie = _mapper.Map<Movie>(request);
        movie.MovieActors = MapActorToMovieActor(request.ActorIds, movie.Id);
        movie.MovieCinemas = MapCinemaToMovieCinema(request.CinemaIds, movie.Id);
        movie.MovieDirectors = MapDirectorToMovieDirector(request.DirectorIds, movie.Id);
        movie.MovieGenres = MapGenreToMovieGenre(request.GenreIds, movie.Id);
        movie.MovieLanguages = MapLanguageToMovieLanguage(request.LanguageIds, movie.Id);
        movie.MovieRatings = MapRatingToMovieRating(request.RatingIds, movie.Id);

        _movieRepository.Add(movie);
        _unitOfWork.SaveChanges();
    }

    public void UpdateMovie(Guid id, UpdateMovieRequest request)
    {
        var movie = GetMovieEntityById(id);
        if (!string.Equals(movie.Title, request.Title, StringComparison.OrdinalIgnoreCase))
            CheckIfMovieExistsByTitle(request.Title);
        var updatedMovie = _mapper.Map(request, movie);
        _movieRepository.Update(updatedMovie);
        _unitOfWork.SaveChanges();
    }

    public void AddMovieActor(Guid id, AddMovieActorRequest request)
    {
        var movie = GetMovieEntityById(id);
        var newMovieActors = MapActorToMovieActor(request.ActorIds, movie.Id);
        newMovieActors.ForEach(x => movie.MovieActors!.Add(x));
        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void AddMovieCinema(Guid id, AddMovieCinemaRequest request)
    {
        var movie = GetMovieEntityById(id);
        var newMovieCinemas = MapCinemaToMovieCinema(request.CinemaIds, movie.Id);
        newMovieCinemas.ForEach(x => movie.MovieCinemas!.Add(x));
        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void AddMovieDirector(Guid id, AddMovieDirectorRequest request)
    {
        var movie = GetMovieEntityById(id);
        var newMovieDirectors = MapDirectorToMovieDirector(request.DirectorIds, movie.Id);
        newMovieDirectors.ForEach(x => movie.MovieDirectors!.Add(x));
        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void AddMovieGenre(Guid id, AddMovieGenreRequest request)
    {
        var movie = GetMovieEntityById(id);
        var newMovieGenres = MapGenreToMovieGenre(request.GenreIds, movie.Id);
        newMovieGenres.ForEach(x => movie.MovieGenres!.Add(x));
        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void AddMovieLanguage(Guid id, AddMovieLanguageRequest request)
    {
        var movie = GetMovieEntityById(id);
        var newMovieLanguages = MapLanguageToMovieLanguage(request.LanguageIds, movie.Id);
        newMovieLanguages.ForEach(x => movie.MovieLanguages!.Add(x));
        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void AddMovieRating(Guid id, AddMovieRatingRequest request)
    {
        var movie = GetMovieEntityById(id);
        var newMovieRatings = MapRatingToMovieRating(request.RatingIds, movie.Id);
        newMovieRatings.ForEach(x => movie.MovieRatings!.Add(x));
        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void DeleteMovie(Guid id)
    {
        var movie = GetMovieEntityById(id);
        _movieRepository.Delete(movie);
        _unitOfWork.SaveChanges();
    }

    public void RemoveMovieActor(Guid id, RemoveMovieActorRequest request)
    {
        var movie = GetMovieEntityById(id);
        var actors = GetActorsByIds(request.ActorIds);
        if (movie.MovieActors is null)
            throw new BusinessException(MovieBusinessMessages.MovieActorsNotFound);

        foreach (var movieActor in actors.Select(actor => movie.MovieActors.FirstOrDefault(x => x.ActorId == actor.Id))
                     .Where(movieActor => movieActor is not null))
        {
            movie.MovieActors.Remove(movieActor!);
        }

        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void RemoveMovieCinema(Guid id, RemoveMovieCinemaRequest request)
    {
        var movie = GetMovieEntityById(id);
        var cinemas = GetCinemasByIds(request.CinemaIds);
        if (movie.MovieCinemas is null)
            throw new BusinessException(MovieBusinessMessages.MovieCinemasNotFound);

        foreach (var movieCinema in cinemas
                     .Select(cinema => movie.MovieCinemas.FirstOrDefault(x => x.CinemaId == cinema.Id))
                     .Where(movieCinema => movieCinema is not null))
        {
            movie.MovieCinemas.Remove(movieCinema!);
        }

        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void RemoveMovieDirector(Guid id, RemoveMovieDirectorRequest request)
    {
        var movie = GetMovieEntityById(id);
        var directors = GetDirectorsByIds(request.DirectorIds);
        if (movie.MovieDirectors is null)
            throw new BusinessException(MovieBusinessMessages.MovieDirectorsNotFound);

        foreach (var movieDirector in directors
                     .Select(director => movie.MovieDirectors.FirstOrDefault(x => x.DirectorId == director.Id))
                     .Where(movieDirector => movieDirector is not null))
        {
            movie.MovieDirectors.Remove(movieDirector!);
        }

        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void RemoveMovieGenre(Guid id, RemoveMovieGenreRequest request)
    {
        var movie = GetMovieEntityById(id);
        var genres = GetGenresByIds(request.GenreIds);
        if (movie.MovieGenres is null)
            throw new BusinessException(MovieBusinessMessages.MovieGenresNotFound);
        
        foreach (var movieGenre in genres
                     .Select(genre => movie.MovieGenres.FirstOrDefault(x => x.GenreId == genre.Id))
                     .Where(movieGenre => movieGenre is not null))
        {
            movie.MovieGenres.Remove(movieGenre!);
        }

        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void RemoveMovieLanguage(Guid id, RemoveMovieLanguageRequest request)
    {
        var movie = GetMovieEntityById(id);
        var languages = GetLanguagesByIds(request.LanguageIds);
        if (movie.MovieLanguages is null)
            throw new BusinessException(MovieBusinessMessages.MovieLanguagesNotFound);

        foreach (var movieLanguage in languages
                     .Select(language => movie.MovieLanguages.FirstOrDefault(x => x.LanguageId == language.Id))
                     .Where(movieLanguage => movieLanguage is not null))
        {
            movie.MovieLanguages.Remove(movieLanguage!);
        }

        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public void RemoveMovieRating(Guid id, RemoveMovieRatingRequest request)
    {
        var movie = GetMovieEntityById(id);
        var ratings = GetRatingsByIds(request.RatingIds);
        if (movie.MovieRatings is null)
            throw new BusinessException(MovieBusinessMessages.MovieRatingsNotFound);

        foreach (var movieRating in ratings
                     .Select(rating => movie.MovieRatings.FirstOrDefault(x => x.RatingId == rating.Id))
                     .Where(movieRating => movieRating is not null))
        {
            movie.MovieRatings.Remove(movieRating!);
        }

        _movieRepository.Update(movie);
        _unitOfWork.SaveChanges();
    }

    public MovieResponse GetMovieById(Guid id)
    {
        var movie = GetMovieEntityById(id);
        return _mapper.Map<MovieResponse>(movie);
    }

    public MovieResponse GetMovieByTitle(string title)
    {
        var movie = _movieRepository.Get(
            predicate: x => x.Title.Equals(title),
            include: source => source
                .Include(x => x.MovieActors).ThenInclude(x => x.Actor)
                .Include(x => x.MovieCinemas).ThenInclude(x => x.Cinema)
                .Include(x => x.MovieDirectors).ThenInclude(x => x.Director)
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieLanguages).ThenInclude(x => x.Language)
                .Include(x => x.MovieRatings).ThenInclude(x => x.Rating)
        );
        if (movie is null)
            throw new NotFoundException(MovieBusinessMessages.MovieNotFoundByTitle);
        return _mapper.Map<MovieResponse>(movie);
    }

    public List<MovieResponse> GetAllMovies()
    {
        var movies = _movieRepository.GetAll(
            include: source => source
                .Include(x => x.MovieActors).ThenInclude(x => x.Actor)
                .Include(x => x.MovieCinemas).ThenInclude(x => x.Cinema)
                .Include(x => x.MovieDirectors).ThenInclude(x => x.Director)
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieLanguages).ThenInclude(x => x.Language)
                .Include(x => x.MovieRatings).ThenInclude(x => x.Rating)
        );

        return _mapper.Map<List<MovieResponse>>(movies);
    }

    private Movie GetMovieEntityById(Guid id)
    {
        var movie = _movieRepository.Get(
            predicate: x => x.Id.Equals(id),
            include: source => source
                .Include(x => x.MovieActors).ThenInclude(x => x.Actor)
                .Include(x => x.MovieCinemas).ThenInclude(x => x.Cinema)
                .Include(x => x.MovieDirectors).ThenInclude(x => x.Director)
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieLanguages).ThenInclude(x => x.Language)
                .Include(x => x.MovieRatings).ThenInclude(x => x.Rating)
        );
        if (movie is null)
            throw new NotFoundException(MovieBusinessMessages.MovieNotFoundById);
        return movie;
    }

    private void CheckIfMovieExistsByTitle(string title)
    {
        var movie = _movieRepository.Get(predicate: x => x.Title.Equals(title));
        if (movie is not null)
            throw new BusinessException(MovieBusinessMessages.MovieAlreadyExistsByName);
    }

    private List<Actor> GetActorsByIds(ICollection<Guid>? actorIds)
    {
        var actors = new List<Actor>();
        if (actorIds is not null)
            actors.AddRange(actorIds.Select(actorId => _actorService.GetActorEntityById(actorId)));
        return actors;
    }

    private List<MovieActor> MapActorToMovieActor(ICollection<Guid>? actorIds, Guid movieId)
    {
        var actors = GetActorsByIds(actorIds);
        if (!actors.Any()) return new List<MovieActor>();
        return actors.Select(actor => new MovieActor
        {
            ActorId = actor.Id,
            MovieId = movieId
        }).ToList();
    }

    private List<Cinema> GetCinemasByIds(ICollection<Guid>? cinemaIds)
    {
        var cinemas = new List<Cinema>();
        if (cinemaIds is not null)
            cinemas.AddRange(cinemaIds.Select(cinemaId => _cinemaService.GetCinemaEntityById(cinemaId)));
        return cinemas;
    }

    private List<MovieCinema> MapCinemaToMovieCinema(ICollection<Guid>? cinemaIds, Guid movieId)
    {
        var cinemas = GetCinemasByIds(cinemaIds);
        if (!cinemas.Any()) return new List<MovieCinema>();
        return cinemas.Select(cinema => new MovieCinema
        {
            CinemaId = cinema.Id,
            MovieId = movieId
        }).ToList();
    }

    private List<Director> GetDirectorsByIds(ICollection<Guid>? directorIds)
    {
        var directors = new List<Director>();
        if (directorIds is not null)
            directors.AddRange(directorIds.Select(directorId => _directorService.GetDirectorEntityById(directorId)));
        return directors;
    }

    private List<MovieDirector> MapDirectorToMovieDirector(ICollection<Guid>? directorIds, Guid movieId)
    {
        var directors = GetDirectorsByIds(directorIds);
        if (!directors.Any()) return new List<MovieDirector>();
        return directors.Select(director => new MovieDirector
        {
            DirectorId = director.Id,
            MovieId = movieId
        }).ToList();
    }

    private List<Genre> GetGenresByIds(ICollection<Guid>? genreIds)
    {
        var genres = new List<Genre>();
        if (genreIds is not null)
            genres.AddRange(genreIds.Select(genreId => _genreService.GetGenreEntityById(genreId)));
        return genres;
    }

    private List<MovieGenre> MapGenreToMovieGenre(ICollection<Guid>? genreIds, Guid movieId)
    {
        var genres = GetGenresByIds(genreIds);
        if (!genres.Any()) return new List<MovieGenre>();
        return genres.Select(genre => new MovieGenre
        {
            GenreId = genre.Id,
            MovieId = movieId
        }).ToList();
    }

    private List<Language> GetLanguagesByIds(ICollection<Guid>? languageIds)
    {
        var languages = new List<Language>();
        if (languageIds is not null)
            languages.AddRange(languageIds.Select(languageId => _languageService.GetLanguageEntityById(languageId)));
        return languages;
    }

    private List<MovieLanguage> MapLanguageToMovieLanguage(ICollection<Guid>? languageIds, Guid movieId)
    {
        var languages = GetLanguagesByIds(languageIds);
        if (!languages.Any()) return new List<MovieLanguage>();
        return languages.Select(language => new MovieLanguage
        {
            LanguageId = language.Id,
            MovieId = movieId
        }).ToList();
    }

    private List<Rating> GetRatingsByIds(ICollection<Guid>? ratingIds)
    {
        var ratings = new List<Rating>();
        if (ratingIds is not null)
            ratings.AddRange(ratingIds.Select(ratingId => _ratingService.GetRatingEntityById(ratingId)));
        return ratings;
    }

    private List<MovieRating> MapRatingToMovieRating(ICollection<Guid>? ratingIds, Guid movieId)
    {
        var ratings = GetRatingsByIds(ratingIds);
        if (!ratings.Any()) return new List<MovieRating>();
        return ratings.Select(rating => new MovieRating
        {
            RatingId = rating.Id,
            MovieId = movieId
        }).ToList();
    }
}