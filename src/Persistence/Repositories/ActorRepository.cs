using Application.Contracts.Repositories;
using Core.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Persistence.Context;

namespace Persistence.Repositories;

public class ActorRepository : BaseRepository<Actor, BaseDbContext>, IActorRepository
{
    public ActorRepository(BaseDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
    }
}