using Application.Contracts.Repositories;
using Core.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Persistence.Context;

namespace Persistence.Repositories;

public class MovieRepository : BaseRepository<Movie, BaseDbContext>, IMovieRepository
{
    public MovieRepository(BaseDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
    }
}