using Core.CrossCuttingConcerns.Logging.Model;

namespace Core.CrossCuttingConcerns.Logging;

public interface ILogModelCreatorService
{
    LogModel LogModel { get; }
    string ConvertToString();
}