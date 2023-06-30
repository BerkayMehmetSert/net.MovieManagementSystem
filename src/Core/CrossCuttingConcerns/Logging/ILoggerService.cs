namespace Core.CrossCuttingConcerns.Logging;

public interface ILoggerService
{
    void Fatal(ILogModelCreatorService service);
    void Error(ILogModelCreatorService service);
    void Warning(ILogModelCreatorService service);
    void Debug(ILogModelCreatorService service);
    void Information(ILogModelCreatorService service);
}