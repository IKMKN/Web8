using Microsoft.AspNetCore.Mvc;

namespace Web8.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate Next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        Next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (Exception ex)
        {
           await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync (HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found!"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request!"),
            _ => (StatusCodes.Status500InternalServerError, "Unknown Exception!")
        };

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        });
    }

}
