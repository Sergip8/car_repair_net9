using System.Security.Claims;

public interface IJwtService
{
    string GenerateToken(int userId, string email, string role);
    ClaimsPrincipal? ValidateToken(string token);
}