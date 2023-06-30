namespace Core.Persistence;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
}