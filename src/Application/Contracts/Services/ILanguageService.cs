using Application.Contracts.Requests.Language;
using Application.Contracts.Responses;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface ILanguageService
{
    void CreateLanguage(CreateLanguageRequest request);
    void UpdateLanguage(Guid id, UpdateLanguageRequest request);
    void DeleteLanguage(Guid id);
    LanguageResponse GetLanguageById(Guid id);
    LanguageResponse GetLanguageByName(string name);
    List<LanguageResponse> GetAllLanguages();
    Language GetLanguageEntityById(Guid id);
}