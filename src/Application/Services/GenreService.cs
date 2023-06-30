using Application.Contracts.Constants.Genre;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Genre;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using AutoMapper;
using Core.Application.Caching;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence;
using Domain.Entities;

namespace Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public GenreService(
        IGenreRepository genreRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ICacheService cacheService
    )
    {
        _genreRepository = genreRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public void CreateGenre(CreateGenreRequest request)
    {
        CheckIfGenreExistsByName(request.Name);
        var genre = _mapper.Map<Genre>(request);
        _genreRepository.Add(genre);
        _unitOfWork.SaveChanges();
        RemoveGenreFromCache(GetCacheKey());
    }

    public void UpdateGenre(Guid id, UpdateGenreRequest request)
    {
        var genre = GetGenreEntityById(id);
        if (!string.Equals(genre.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            CheckIfGenreExistsByName(request.Name);

        var updatedGenre = _mapper.Map(request, genre);
        _genreRepository.Update(updatedGenre);
        _unitOfWork.SaveChanges();
        RemoveGenreFromCache(GetCacheKey(id));
    }

    public void DeleteGenre(Guid id)
    {
        var genre = GetGenreEntityById(id);
        _genreRepository.Delete(genre);
        _unitOfWork.SaveChanges();
        RemoveGenreFromCache(GetCacheKey(id));
    }

    public GenreResponse GetGenreById(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        var genreFromCache = GetGenresFromCache(cacheKey);
        if (genreFromCache is not null)
            return genreFromCache.FirstOrDefault()!;

        var genre = GetGenreEntityById(id);
        var response = _mapper.Map<GenreResponse>(genre);
        SetGenreToCache(cacheKey, new List<GenreResponse> { response });
        return response;
    }

    public GenreResponse GetGenreByName(string name)
    {
        var cacheKey = GetCacheKey(name);
        var genreFromCache = GetGenresFromCache(cacheKey);
        if (genreFromCache is not null)
            return genreFromCache.FirstOrDefault()!;

        var genre = _genreRepository.Get(predicate: x => x.Name.Equals(name));
        if (genre is null)
            throw new NotFoundException(GenreBusinessMessages.GenreNotFoundByName);

        var response = _mapper.Map<GenreResponse>(genre);
        SetGenreToCache(cacheKey, new List<GenreResponse> { response });
        return response;
    }

    public List<GenreResponse> GetAllGenres()
    {
        var cacheKey = GetCacheKey();
        var genresFromCache = GetGenresFromCache(cacheKey);
        if (genresFromCache is not null)
            return genresFromCache;

        var genres = _genreRepository.GetAll();
        var response = _mapper.Map<List<GenreResponse>>(genres);
        SetGenreToCache(cacheKey, response);
        return response;
    }

    public Genre GetGenreEntityById(Guid id)
    {
        var genre = _genreRepository.Get(predicate: x => x.Id.Equals(id));
        return genre ?? throw new NotFoundException(GenreBusinessMessages.GenreNotFoundById);
    }

    private void CheckIfGenreExistsByName(string name)
    {
        var genre = _genreRepository.Get(predicate: x => x.Name.Equals(name));
        if (genre is not null)
            throw new BusinessException(GenreBusinessMessages.GenreAlreadyExistsByName);
    }

    private static string GetCacheKey() => "Genre:All";

    private static string GetCacheKey(object value) => $"Genre:{value}";

    private List<GenreResponse>? GetGenresFromCache(string cacheKey)
    {
        return _cacheService.TryGet(cacheKey, out List<GenreResponse>? genres) ? genres : null;
    }

    private void SetGenreToCache(string cacheKey, List<GenreResponse> genres) => _cacheService.Set(cacheKey, genres);

    private void RemoveGenreFromCache(string cacheKey) => _cacheService.Remove(cacheKey);
}