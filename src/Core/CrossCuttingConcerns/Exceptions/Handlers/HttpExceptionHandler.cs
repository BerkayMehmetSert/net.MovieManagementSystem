using System.Text.Json;
using Core.CrossCuttingConcerns.Exceptions.Details;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers;

public class HttpExceptionHandler : ExceptionHandler
{
    public HttpResponse? Response { get; set; }

    protected override Task HandleException(BusinessException businessException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        var detail = new BusinessExceptionDetails(businessException.Message);
        var result = JsonSerializer.Serialize(detail);
        return Response.WriteAsync(result);
    }

    protected override Task HandleException(NotFoundException notFoundException)
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        var detail = new NotFoundExceptionDetails(notFoundException.Message);
        var result = JsonSerializer.Serialize(detail);
        return Response.WriteAsync(result);
    }

    protected override Task HandleException(Exception exception)
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        var detail = new InternalServerErrorExceptionDetails(exception.Message);
        var result = JsonSerializer.Serialize(detail);
        return Response.WriteAsync(result);
    }
}