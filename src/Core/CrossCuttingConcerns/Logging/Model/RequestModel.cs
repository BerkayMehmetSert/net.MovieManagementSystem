namespace Core.CrossCuttingConcerns.Logging.Model;

public class RequestModel
{
    public string? RequestPath { get; set; }
    public string? RequestQuery { get; set; }
    public List<KeyValuePair<string, string>>? RequestQueries { get; set; }
    public string? RequestMethod { get; set; }
    public string? RequestScheme { get; set; }
    public string? RequestHost { get; set; }
    public Dictionary<string, string>? RequestHeaders { get; set; }
    public string RequestBody { get; set; }
    public string? RequestContentType { get; set; }
}