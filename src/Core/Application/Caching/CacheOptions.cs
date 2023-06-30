namespace Core.Application.Caching;

public class CacheOptions
{
    public int AbsoluteExpirationInHours { get; set; }
    public int SlidingExpirationInMinutes { get; set; }
}