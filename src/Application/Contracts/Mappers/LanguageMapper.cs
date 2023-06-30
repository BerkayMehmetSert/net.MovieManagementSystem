using Application.Contracts.Requests.Language;
using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mappers;

public class LanguageMapper : Profile
{
    public LanguageMapper()
    {
        CreateMap<CreateLanguageRequest, Language>();
        CreateMap<UpdateLanguageRequest, Language>();
        CreateMap<Language, LanguageResponse>();
    }
}