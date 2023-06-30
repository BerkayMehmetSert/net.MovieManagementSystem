using Core.Domain;

namespace Core.Test.FakeData;

public abstract class BaseFakeData<TEntity>
{
    public List<TEntity> Data => CreateFakeData();
    public abstract List<TEntity> CreateFakeData();
}