public interface IExceptionHandlingService
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> operation, string operationName = null);
    Task ExecuteAsync(Func<Task> operation, string operationName = null);
    T Execute<T>(Func<T> operation, string operationName = null);
    void Execute(Action operation, string operationName = null);
}