using System.Net;
using System.Text.Json;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationMiddleware> _logger;

    public AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        // Interceptar respuestas de autorización
        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            await HandleUnauthorizedAsync(context);
        }
        else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            await HandleForbiddenAsync(context);
        }
    }

    private async Task HandleUnauthorizedAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        
        var response = new ErrorResponse
        {
            StatusCode = 401,
            Message = "Token de acceso requerido",
            Details = new Dictionary<string, object>
            {
                { "ErrorDetail", "Debes proporcionar un token de autenticación válido para acceder a este recurso" }
            },
            Timestamp = DateTime.UtcNow,
            Path = context.Request.Path
        };

        _logger.LogWarning("Acceso no autorizado a {Path} desde {RemoteIp}", 
            context.Request.Path, context.Connection.RemoteIpAddress);

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private async Task HandleForbiddenAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var userRole = context.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "Sin rol";
        var userName = context.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "Usuario desconocido";
        
        var response = new ErrorResponse
        {
            StatusCode = 403,
            Message = "Acceso denegado - Rol insuficiente",
            Details = new Dictionary<string, object>
            {
                { "ErrorDetail", $"Tu rol actual '{userRole}' no tiene permisos para acceder a este recurso" }
            },
            Timestamp = DateTime.UtcNow,
            Path = context.Request.Path,
            UserInfo = new UserInfo
            {
                Email = userName,
                Role = userRole
            }
        };

        _logger.LogWarning("Acceso denegado para usuario {UserName} con rol {UserRole} a {Path}", 
            userName, userRole, context.Request.Path);

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

