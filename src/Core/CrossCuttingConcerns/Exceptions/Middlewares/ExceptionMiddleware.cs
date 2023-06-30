using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Date;
using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Exceptions.Middlewares;

public class ExceptionMiddleware
{
    private readonly HttpExceptionHandler _httpExceptionHandler;

    private readonly ILoggerService _loggerService;

    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILoggerService loggerService)
    {
        _httpExceptionHandler = new HttpExceptionHandler();
        _next = next;
        _loggerService = loggerService;
    }

    public async Task InvokeAsync(HttpContext context, ILogModelCreatorService creatorService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleLogException(context, exception, creatorService);
            await HandleExceptionAsync(context.Response, exception);
        }
    }

    private Task HandleExceptionAsync(HttpResponse response, Exception exception)
    {
        response.ContentType = "application/json";
        _httpExceptionHandler.Response = response;
        return _httpExceptionHandler.HandleExceptionAsync(exception);
    }

    private Task HandleLogException(HttpContext context, Exception exception, ILogModelCreatorService creatorService)
    {
        var log = creatorService.LogModel;

        log.RequestDateTimeUtc = DateHelper.GetCurrentDate();
        log.TraceId = context.TraceIdentifier;
        var ip = context.Connection.RemoteIpAddress;
        log.ClientIp = ip?.ToString();
        log.Node = Environment.MachineName;
        log.ExceptionMessage = exception.Message;
        log.ExceptionStackTrace = exception.StackTrace;

        _loggerService.Error(creatorService);

        return Task.CompletedTask;
    }
}