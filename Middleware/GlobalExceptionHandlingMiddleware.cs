using System.Diagnostics;
using System.Text.Json;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        
        // Log the exception
        LogException(exception, traceId, context);

        // Create error response
        var errorResponse = CreateErrorResponse(exception, traceId);
        
        // Set response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorResponse.StatusCode;

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private void LogException(Exception exception, string traceId, HttpContext context)
    {
        var logLevel = exception switch
        {
            ValidationException or NotFoundException or BusinessLogicException => LogLevel.Warning,
            _ => LogLevel.Error
        };

        _logger.Log(logLevel, exception, 
            "Exception occurred. TraceId: {TraceId}, Path: {Path}, Method: {Method}",
            traceId, context.Request.Path, context.Request.Method);
    }

    private ErrorResponse CreateErrorResponse(Exception exception, string traceId)
    {
        return exception switch
        {
            ValidationException validationEx => new ValidationErrorResponse
            {
                Message = validationEx.Message,
                ErrorCode = validationEx.ErrorCode,
                StatusCode = validationEx.StatusCode,
                TraceId = traceId,
                ValidationErrors = validationEx.Errors
            },
            BaseException baseEx => new ErrorResponse
            {
                Message = baseEx.Message,
                ErrorCode = baseEx.ErrorCode,
                StatusCode = baseEx.StatusCode,
                TraceId = traceId
            },
            _ => new ErrorResponse
            {
                Message = "An unexpected error occurred.",
                ErrorCode = "INTERNAL_ERROR",
                StatusCode = 500,
                TraceId = traceId
            }
        };
    }
}
