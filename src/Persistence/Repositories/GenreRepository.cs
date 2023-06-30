using Application.Contracts.Repositories;
using Core.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Persistence.Context;

namespace Persistence.Repositories;

public class GenreRepository : BaseRepository<Genre, BaseDbContext>, IGenreRepository
{
    public GenreRepository(BaseDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
    }
}