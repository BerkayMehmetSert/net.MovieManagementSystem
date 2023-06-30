using Serilog;

namespace Core.CrossCuttingConcerns.Logging;

public class LoggerService : ILoggerService
{
    public void Fatal(ILogModelCreatorService service) => Log.Fatal(service.ConvertToString());

    public void Error(ILogModelCreatorService service) => Log.Error(service.ConvertToString());

    public void Warning(ILogModelCreatorService service) => Log.Warning(service.ConvertToString());

    public void Debug(ILogModelCreatorService service) => Log.Debug(service.ConvertToString());

    public void Information(ILogModelCreatorService service) => Log.Information(service.ConvertToString());
}