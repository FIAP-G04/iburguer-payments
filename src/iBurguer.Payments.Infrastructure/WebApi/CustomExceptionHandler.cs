using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static iBurguer.Payments.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace iBurguer.Payments.Infrastructure.WebApi;

[ExcludeFromCodeCoverage]
public sealed class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exceptionMessage}, Occurred at: {time}",
            exception.Message, DateTime.UtcNow);

        int statusCode = GetStatusCodeFromException(exception);

        ProblemDetails problemDetails = new()
        {
            Type = "https://httpstatuses.com/" + statusCode,
            Title = exception.GetType().Name,
            Detail = exception.Message,
            Status = statusCode,
            Instance = httpContext.Request.Path
        };
        
        problemDetails.Extensions.Add(new KeyValuePair<string, object?>("traceId", Activity.Current?.Id ?? httpContext.TraceIdentifier));
        
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private int GetStatusCodeFromException(Exception exception) => exception switch
    {
        CannotToConfirmPayment => StatusCodes.Status422UnprocessableEntity,
        CannotToRefusePayment => StatusCodes.Status422UnprocessableEntity,
        InvalidAmount => StatusCodes.Status422UnprocessableEntity,
        PaymentNotFound => StatusCodes.Status404NotFound,

        _ => StatusCodes.Status500InternalServerError
    };
}