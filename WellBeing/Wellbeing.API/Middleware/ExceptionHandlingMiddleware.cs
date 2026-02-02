using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using FluentValidation;

namespace Wellbeing.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred. Path: {Path}, Method: {Method}", context.Request.Path, context.Request.Method);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationEx:
                code = HttpStatusCode.BadRequest;
                var validationErrors = validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                result = JsonSerializer.Serialize(new 
                { 
                    error = "Validation failed. Please check the errors below.",
                    errors = validationErrors
                });
                break;
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { error = exception.Message });
                break;
            case ArgumentException:
            case InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { error = exception.Message });
                break;
            case DbUpdateException dbEx when dbEx.InnerException is PostgresException pgEx:
                if (pgEx.SqlState == "23505")
                {
                    code = HttpStatusCode.Conflict;
                    var errorMessage = "A record with this value already exists.";
                    
                    if (pgEx.ConstraintName != null)
                    {
                        if (pgEx.ConstraintName.Contains("Domain"))
                        {
                            errorMessage = "A client with this domain already exists. Please use a different domain.";
                        }
                        else if (pgEx.ConstraintName.Contains("Email"))
                        {
                            errorMessage = "A user with this email address already exists. Please use a different email.";
                        }
                        else if (pgEx.ConstraintName.Contains("UserName"))
                        {
                            errorMessage = "A user with this username already exists. Please choose a different username.";
                        }
                        else
                        {
                            errorMessage = $"A record with this value already exists. Constraint: {pgEx.ConstraintName}";
                        }
                    }
                    
                    result = JsonSerializer.Serialize(new { error = errorMessage });
                }
                else if (pgEx.SqlState == "23503")
                {
                    code = HttpStatusCode.BadRequest;
                    var errorMessage = "The referenced record does not exist or has been deleted.";
                    
                    if (pgEx.ConstraintName != null)
                    {
                        if (pgEx.ConstraintName.Contains("Clients"))
                        {
                            errorMessage = "The specified client does not exist or has been deleted.";
                        }
                        else if (pgEx.ConstraintName.Contains("WellbeingDimension"))
                        {
                            errorMessage = "The specified wellbeing dimension does not exist or has been deleted.";
                        }
                        else if (pgEx.ConstraintName.Contains("WellbeingSubDimension"))
                        {
                            errorMessage = "The specified wellbeing sub-dimension does not exist or has been deleted.";
                        }
                        else if (pgEx.ConstraintName.Contains("Question"))
                        {
                            errorMessage = "The specified question does not exist or has been deleted.";
                        }
                        else if (pgEx.ConstraintName.Contains("Survey"))
                        {
                            errorMessage = "The specified survey does not exist or has been deleted.";
                        }
                        else if (pgEx.ConstraintName.Contains("AspNetUsers"))
                        {
                            errorMessage = "The specified user does not exist or has been deleted.";
                        }
                    }
                    
                    result = JsonSerializer.Serialize(new { error = errorMessage });
                }
                else
                {
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { error = "A database error occurred while processing your request. Please try again later." });
                }
                break;
            default:
                result = JsonSerializer.Serialize(new { error = "An error occurred while processing your request." });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
