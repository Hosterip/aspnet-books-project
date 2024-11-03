using BooksApp.Contracts.Responses.Errors;
using FluentValidation;

namespace BooksApp.API.Middlewares;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            var errors = exception.Errors.Select(x => new ValidationError
            {
                ErrorMessage = x.ErrorMessage,
                PropertyName = x.PropertyName
            });
            var response = new ValidationFailureResponse
            {
                Errors = errors
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}