using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace common.exception;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Exception ex)
        {

            await HandleException(ex, context);
        }
    }

    private async Task HandleException(System.Exception ex, HttpContext context)
    {
        ApiError response;
        int statusCode = StatusCodes.Status500InternalServerError;
        string message;
        var exceptionType = ex.GetType();

        if (exceptionType == typeof(UnauthorizedAccessException))
        {
            statusCode = StatusCodes.Status403Forbidden;
            message = "You are not authorized";
        }
        else if (exceptionType == typeof(FailedServiceException))
        {
            statusCode = StatusCodes.Status400BadRequest;
            message = ex.Message;
        }
        else if(exceptionType.Name == "BadHttpRequestException")
        {
            statusCode = StatusCodes.Status400BadRequest;
            message = ex.Message;
        }
        else
        {
            message = ex.Message;
        }

        if (_env.IsDevelopment())
        {
            response = new ApiError(statusCode, ex.Message, ex.StackTrace.ToString());
        }
        else
        {
            response = new ApiError(statusCode, message);
        }

        _logger.LogError(ex, ex.Message);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(response.ToString());
    }
}
