using System.Linq.Expressions;
using Core.Domain;
using Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace Core.Test.Helper;

public class MockRepositoryHelper
{
    public static Mock<TRepository> GetRepository<TRepository, TEntity>(List<TEntity> list)
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        var mockRepo = new Mock<TRepository>();
        Build(mockRepo, list);
        return mockRepo;
    }

    private static void Build<TRepository, TEntity>(Mock<TRepository> mockRepo, List<TEntity> list)
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        SetupGetAll(mockRepo, list);
        SetupGet(mockRepo, list);
        SetupAdd(mockRepo, list);
        SetupUpdate(mockRepo, list);
        SetupDelete(mockRepo, list);
        SetupSoftDelete(mockRepo, list);
    }

    private static void SetupGetAll<TRepository, TEntity>(Mock<TRepository> mockRepo, List<TEntity> list)
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        mockRepo.Setup(
            x => x.GetAll(
                It.IsAny<Expression<Func<TEntity, bool>>>(),
                It.IsAny<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>(),
                It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>(),
                It.IsAny<bool>()
            )
        ).Returns(
            (Expression<Func<TEntity, bool>> expression,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
                bool enableTracking
            ) =>
            {
                var result = list;
                if (!enableTracking)
                    result = list.AsQueryable().AsNoTracking().ToList();
                if (expression is not null)
                    result = result.Where(expression.Compile()).ToList();
                if (include is not null)
                    result = include(result.AsQueryable()).ToList();
                if (orderBy is not null)
                    result = orderBy(result.AsQueryable()).ToList();
                return result;
            }
        );
    }

    private static void SetupGet<TRepository, TEntity>(Mock<TRepository> mockRepo, List<TEntity> list)
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        mockRepo.Setup(
            x => x.Get(
                It.IsAny<Expression<Func<TEntity, bool>>>(),
                It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>(),
                It.IsAny<bool>()
            )).Returns(
            (Expression<Func<TEntity, bool>> expression,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
                bool enableTracking) =>
            {
                var result = list;
                if (!enableTracking)
                    result = list.AsQueryable().AsNoTracking().ToList();
                if (include is not null)
                    result = include(result.AsQueryable()).ToList();
                return result.FirstOrDefault(expression.Compile());
            }
        );
    }

    private static void SetupAdd<TRepository, TEntity>(Mock<TRepository> mockRepo, ICollection<TEntity> list)
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        mockRepo.Setup(x => x.Add(It.IsAny<TEntity>()))
            .Callback((TEntity entity) => list.Add(entity));
    }

    private static void SetupUpdate<TRepository, TEntity>(Mock<TRepository> mockRepo, ICollection<TEntity> list)
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        mockRepo.Setup(x => x.Update(It.IsAny<TEntity>()))
            .Callback((TEntity entity) =>
            {
                var item = list.FirstOrDefault(x => x.Id == entity.Id);
                if (item is not null)
                {
                    list.Remove(item);
                    list.Add(entity);
                }
            });
    }

    private static void SetupDelete<TRepository, TEntity>(Mock<TRepository> mockRepo, ICollection<TEntity> list)
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        mockRepo.Setup(x => x.Delete(It.IsAny<TEntity>()))
            .Callback((TEntity entity) =>
            {
                var item = list.FirstOrDefault(x => x.Id == entity.Id);
                if (item is not null)
                    list.Remove(item);
            });
    }
    
    private static void SetupSoftDelete<TRepository, TEntity>(Mock<TRepository> mockRepo, ICollection<TEntity> list)
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        mockRepo.Setup(x => x.SoftDelete(It.IsAny<TEntity>()))
            .Callback((TEntity entity) =>
            {
                var item = list.FirstOrDefault(x => x.Id == entity.Id);
                if (item is not null)
                {
                    list.Remove(item);
                    list.Add(entity);
                }
            });
    }
}