public class ExceptionHandlingService : IExceptionHandlingService
{
    private readonly ILogger<ExceptionHandlingService> _logger;

    public ExceptionHandlingService(ILogger<ExceptionHandlingService> logger)
    {
        _logger = logger;
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, string operationName)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            HandleException(ex, operationName);
            throw; // Re-throw to be handled by global middleware
        }
    }

    public async Task ExecuteAsync(Func<Task> operation, string operationName)
    {
        try
        {
            await operation();
        }
        catch (Exception ex)
        {
            HandleException(ex, operationName);
            throw;
        }
    }

    public T Execute<T>(Func<T> operation, string operationName)
    {
        try
        {
            return operation();
        }
        catch (Exception ex)
        {
            HandleException(ex, operationName);
            throw;
        }
    }

    public void Execute(Action operation, string operationName)
    {
        try
        {
            operation();
        }
        catch (Exception ex)
        {
            HandleException(ex, operationName);
            throw;
        }
    }

    private void HandleException(Exception exception, string operationName)
    {
        // Transform common exceptions
        var transformedException = TransformException(exception, operationName);
        
        if (transformedException != exception)
        {
            _logger.LogWarning("Exception transformed from {OriginalType} to {NewType} in operation {OperationName}", 
                exception.GetType().Name, transformedException.GetType().Name, operationName);
        }
    }

    private Exception TransformException(Exception exception, string operationName)
    {
        return exception switch
        {
           
            ArgumentNullException argEx => new ValidationException(
                $"Required parameter '{argEx.ParamName}' is missing."),
            ArgumentException argEx => new ValidationException(argEx.Message),
            InvalidOperationException invalidOpEx => new BusinessLogicException(invalidOpEx.Message),
            _ => exception
        };
    }
}