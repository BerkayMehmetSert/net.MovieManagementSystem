using Application.Contracts.Repositories;
using Core.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Persistence.Context;

namespace Persistence.Repositories;

public class LanguageRepository : BaseRepository<Language, BaseDbContext>, ILanguageRepository
{
    public LanguageRepository(BaseDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
    }
}