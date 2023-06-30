namespace Core.CrossCuttingConcerns.Logging.Model;

public class LogModel
{
    public string Node { get; set; }
    public string? ClientIp { get; set; }
    public string TraceId { get; set; }
    public DateTime? RequestDateTimeUtc { get; set; }
    public RequestModel Request { get; set; }
    public string ExceptionMessage { get; set; }
    public string? ExceptionStackTrace { get; set; }
    
    public LogModel()
    {
        Request = new RequestModel();
    }
}