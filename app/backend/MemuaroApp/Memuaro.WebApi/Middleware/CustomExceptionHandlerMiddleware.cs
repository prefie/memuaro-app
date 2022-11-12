using System.Net;
using System.Text.Json;
using Memuaro.Auth.Exceptions;
using Memuaro.WebApi.Exceptions;

namespace Memuaro.WebApi.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        switch (exception)
        {
            case UnauthorizedException unauthorizedException:
                code = HttpStatusCode.Unauthorized;
                break;
            case ForbiddenException forbiddenException:
                code = HttpStatusCode.Forbidden;
                break;
            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) code;

        if (result == string.Empty) result = JsonSerializer.Serialize(new {Error = exception.Message});

        return context.Response.WriteAsync(result);
    }
}