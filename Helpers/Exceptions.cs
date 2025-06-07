public abstract class BaseException : Exception
{
    public string ErrorCode { get; }
    public int StatusCode { get; }
    
    protected BaseException(string message, string errorCode, int statusCode) 
        : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
    
    protected BaseException(string message, Exception innerException, string errorCode, int statusCode) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}

public class ValidationException : BaseException
{
    public Dictionary<string, string[]> Errors { get; }
    
    public ValidationException(string message, Dictionary<string, string[]> errors = null) 
        : base(message, "VALIDATION_ERROR", 400)
    {
        Errors = errors ?? new Dictionary<string, string[]>();
    }
}

public class NotFoundException : BaseException
{
    public NotFoundException(string message) 
        : base(message, "NOT_FOUND", 404) { }
}

public class BusinessLogicException : BaseException
{
    public BusinessLogicException(string message) 
        : base(message, "BUSINESS_LOGIC_ERROR", 422) { }
}

public class DatabaseException : BaseException
{
    public DatabaseException(string message, Exception innerException)
        : base(message, innerException, "DATABASE_ERROR", 500) { }
}

public static class ExceptionExtensions
{
    public static void ThrowIfNotFound<T>(this T entity, string entityName, object id) where T : class
    {
        if (entity == null)
            throw new NotFoundException($"{entityName} with ID '{id}' was not found.");
    }

    public static void ThrowIfNull(this object value, string parameterName)
    {
        if (value == null)
            throw new ValidationException($"Parameter '{parameterName}' cannot be null.");
    }

    public static void ThrowIfNullOrEmpty(this string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"Parameter '{parameterName}' cannot be null or empty.");
    }
}