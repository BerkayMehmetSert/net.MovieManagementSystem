using System.Linq.Expressions;
using System.Security.Claims;
using Core.Domain;
using Core.Utilities.Date;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Persistence;

public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly DbSet<TEntity> _entities;

    public BaseRepository(TContext context, IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _contextAccessor = contextAccessor;
        _entities = context.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        entity.CreatedAt = DateHelper.GetCurrentDate();
        entity.CreatedBy = _contextAccessor.HttpContext?.User.FindFirstValue("FullName") ?? "System";
        _entities.Add(entity);
    }

    public void Update(TEntity entity)
    {
        entity.UpdatedAt = DateHelper.GetCurrentDate();
        entity.UpdatedBy = _contextAccessor.HttpContext?.User.FindFirstValue("FullName") ?? "System";
        _entities.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public void SoftDelete(TEntity entity)
    {
        Update(entity);
    }

    public TEntity Get(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        bool enableTracking = true
    )
    {
        IQueryable<TEntity> query = _entities;

        if (!enableTracking) query = query.AsNoTracking();
        
        if (include is not null) query = include(query);

        return query.FirstOrDefault(predicate);
    }

    public List<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool enableTracking = true
    )
    {
        IQueryable<TEntity> query = _entities;

        if (!enableTracking) query = query.AsNoTracking();
        if (predicate is not null) query = query.Where(predicate);
        if (include is not null) query = include(query);

        return orderBy is not null ? orderBy(query).ToList() : query.ToList();
    }
}