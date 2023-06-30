using Core.CrossCuttingConcerns.Logging.Constants;
using Core.CrossCuttingConcerns.Logging.Model;
using Core.Utilities.Date;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Core.CrossCuttingConcerns.Logging.Middleware;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerService _loggerService;

    public LoggerMiddleware(RequestDelegate next, ILoggerService loggerService)
    {
        _next = next;
        _loggerService = loggerService;
    }

    public async Task Invoke(HttpContext context, ILogModelCreatorService creatorService)
    {
        var log = creatorService.LogModel;

        log.RequestDateTimeUtc = DateHelper.GetCurrentDate();
        var request = context.Request;
        log.TraceId = context.TraceIdentifier;
        var ip = context.Connection.RemoteIpAddress;
        log.ClientIp = ip?.ToString();
        log.Node = Environment.MachineName;
        log.Request.RequestMethod = request.Method;
        log.Request.RequestPath = request.Path;
        log.Request.RequestQuery = request.QueryString.ToString();
        log.Request.RequestQueries = FormatQueries(request.QueryString.ToString());
        log.Request.RequestHeaders = FilterSensitiveHeaders(request.Headers);

        var requestBody = await ReadBodyFromRequest(request);
        log.Request.RequestBody = FilterSensitiveData(requestBody);
        log.Request.RequestScheme = request.Scheme;
        log.Request.RequestHost = request.Host.ToString();
        log.Request.RequestContentType = request.ContentType;

        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            LogErrorHandler(log, exception);
        }

        var contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (contextFeature?.Error is not null)
        {
            var exception = contextFeature.Error;
            LogErrorHandler(log, exception);
        }

        _loggerService.Information(creatorService);
    }

    private static void LogErrorHandler(LogModel log, Exception exception)
    {
        log.ExceptionMessage = exception.Message;
        log.ExceptionStackTrace = exception.StackTrace;
    }

    private static List<KeyValuePair<string, string>> FormatQueries(string queryString)
    {
        return queryString.TrimStart('?')
            .Split("&")
            .Select(query => query.Split("="))
            .Where(items => items.Length >= 1)
            .Select(items => new KeyValuePair<string, string>(items[0], items.Length >= 2 ? items[1] : string.Empty))
            .ToList();
    }

    private static async Task<string?> ReadBodyFromRequest(HttpRequest request)
    {
        request.EnableBuffering();
        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        string requestBody = await streamReader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return requestBody;
    }

    private static string FilterSensitiveData(string data)
    {
        if (string.IsNullOrEmpty(data)) return data;

        var jsonObject = JObject.Parse(data);

        foreach (var token in from field in MiddlewareConstants.SensitiveFields
                 select jsonObject.SelectToken("$.." + field)
                 into token
                 where token is not null
                 where token.Type == JTokenType.String
                 select token)
        {
            token.Replace(new JValue("[FILTERED]"));
        }

        return jsonObject.ToString();
    }
    
    private Dictionary<string, string> FilterSensitiveHeaders(IHeaderDictionary headers)
    {
        var filteredHeaders = new Dictionary<string, string>();

        foreach (var header in headers)
        {
            var headerName = header.Key;
            var headerValue = MiddlewareConstants.SensitiveFields.Contains(headerName)
                ? "[FILTERED]"
                : header.Value.ToString();
            filteredHeaders.Add(headerName, headerValue);
        }

        return filteredHeaders;
    }
}