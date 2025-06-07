public class ErrorResponse
{
    public string Message { get; set; }
    public string ErrorCode { get; set; }
    public int StatusCode { get; set; }
    public string TraceId { get; set; }
    public string? Path { get; set; }

    public DateTime Timestamp { get; set; }
    public Dictionary<string, object> Details { get; set; }

    public UserInfo? UserInfo { get; set; }

    public ErrorResponse()
    {
        Timestamp = DateTime.UtcNow;
        Details = new Dictionary<string, object>();
    }

}

public class UserInfo
{
    public string? Email { get; set; }
    public string? Role { get; set; }
}

public class ValidationErrorResponse : ErrorResponse
{
    public Dictionary<string, string[]> ValidationErrors { get; set; }

    public ValidationErrorResponse()
    {
        ValidationErrors = new Dictionary<string, string[]>();
    }
}