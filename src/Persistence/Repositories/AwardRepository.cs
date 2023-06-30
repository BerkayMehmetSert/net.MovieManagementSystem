using Application.Contracts.Repositories;
using Core.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Persistence.Context;

namespace Persistence.Repositories;

public class AwardRepository : BaseRepository<Award, BaseDbContext>, IAwardRepository
{
    public AwardRepository(BaseDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
    }
}