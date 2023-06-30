using Application.Contracts.Constants.Language;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Language;
using Application.Contracts.Responses;
using Application.Contracts.Services;
using AutoMapper;
using Core.Application.Caching;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence;
using Domain.Entities;

namespace Application.Services;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public LanguageService(
        ILanguageRepository languageRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ICacheService cacheService
    )
    {
        _languageRepository = languageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public void CreateLanguage(CreateLanguageRequest request)
    {
        CheckIfLanguageExistsByName(request.Name);
        var language = _mapper.Map<Language>(request);
        _languageRepository.Add(language);
        _unitOfWork.SaveChanges();
        RemoveLanguageFromCache(GetCacheKey());
    }

    public void UpdateLanguage(Guid id, UpdateLanguageRequest request)
    {
        var language = GetLanguageEntityById(id);
        if (!string.Equals(language.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            CheckIfLanguageExistsByName(request.Name);

        var updatedLanguage = _mapper.Map(request, language);
        _languageRepository.Update(updatedLanguage);
        _unitOfWork.SaveChanges();
        RemoveLanguageFromCache(GetCacheKey(id));
    }

    public void DeleteLanguage(Guid id)
    {
        var language = GetLanguageEntityById(id);
        _languageRepository.Delete(language);
        _unitOfWork.SaveChanges();
        RemoveLanguageFromCache(GetCacheKey(id));
    }

    public LanguageResponse GetLanguageById(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        var languagesFromCache = GetLanguagesFromCache(cacheKey);
        if (languagesFromCache is not null)
            return languagesFromCache.FirstOrDefault()!;

        var language = GetLanguageEntityById(id);
        var response = _mapper.Map<LanguageResponse>(language);
        SetLanguageToCache(cacheKey, new List<LanguageResponse> { response });
        return response;
    }

    public LanguageResponse GetLanguageByName(string name)
    {
        var cacheKey = GetCacheKey(name);
        var languagesFromCache = GetLanguagesFromCache(cacheKey);
        if (languagesFromCache is not null)
            return languagesFromCache.FirstOrDefault()!;

        var language = _languageRepository.Get(predicate: x => x.Name.Equals(name));
        if (language is null)
            throw new NotFoundException(LanguageBusinessMessages.LanguageNotFoundByName);
        var response = _mapper.Map<LanguageResponse>(language);
        SetLanguageToCache(cacheKey, new List<LanguageResponse> { response });
        return response;
    }

    public List<LanguageResponse> GetAllLanguages()
    {
        var cacheKey = GetCacheKey();
        var languagesFromCache = GetLanguagesFromCache(cacheKey);
        if (languagesFromCache is not null)
            return languagesFromCache;

        var languages = _languageRepository.GetAll();
        var response = _mapper.Map<List<LanguageResponse>>(languages);
        SetLanguageToCache(cacheKey, response);
        return response;
    }

    public Language GetLanguageEntityById(Guid id)
    {
        var language = _languageRepository.Get(predicate: x => x.Id.Equals(id));
        if (language is null)
            throw new NotFoundException(LanguageBusinessMessages.LanguageNotFoundById);
        return _mapper.Map<Language>(language);
    }

    private void CheckIfLanguageExistsByName(string name)
    {
        var language = _languageRepository.Get(predicate: x => x.Name.Equals(name));
        if (language is not null)
            throw new BusinessException(LanguageBusinessMessages.LanguageAlreadyExistsByName);
    }

    private static string GetCacheKey() => "Language:All";

    private static string GetCacheKey(object value) => $"Language:{value}";

    private List<LanguageResponse>? GetLanguagesFromCache(string cacheKey)
    {
        return _cacheService.TryGet(cacheKey, out List<LanguageResponse>? languages) ? languages : null;
    }

    private void SetLanguageToCache(string cacheKey, List<LanguageResponse> languages)
    {
        _cacheService.Set(cacheKey, languages);
    }

    private void RemoveLanguageFromCache(string cacheKey) => _cacheService.Remove(cacheKey);
}