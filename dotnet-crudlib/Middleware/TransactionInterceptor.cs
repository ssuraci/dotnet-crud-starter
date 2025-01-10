using Microsoft.EntityFrameworkCore;
using Castle.DynamicProxy;

namespace NetCrudStarter.Middleware;


public class TransactionInterceptor : IInterceptor
{
    private readonly DbContext _dbContext;
    private readonly ILogger<TransactionInterceptor> _logger;

    public TransactionInterceptor(DbContext dbContext, ILogger<TransactionInterceptor> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        // Determine if the method is async
        if (typeof(Task).IsAssignableFrom(invocation.Method.ReturnType))
        {
            // Handle async methods
            HandleAsync(invocation);
        }
        else
        {
            // Handle synchronous methods
            HandleSync(invocation);
        }
    }

    protected bool hasTransactionalAttribute(IInvocation invocation)
    {
        return invocation.MethodInvocationTarget.GetCustomAttributes(typeof(TransactionalAttribute), true).Length > 0;
    }
    
    private void HandleSync(IInvocation invocation)
    {
        if (hasTransactionalAttribute(invocation) && _dbContext.Database.CurrentTransaction is null)
        {
            _logger.LogInformation("Starting transaction for method {0} sync", invocation.Method.Name);
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                invocation.Proceed(); // Execute the method
                _logger.LogInformation("Commit transaction for method {0} sync", invocation.Method.Name);
                transaction.Commit(); // Commit transaction on success
            }
            catch
            {
                _logger.LogInformation("Rollback transaction for method {0} sync", invocation.Method.Name);
                transaction.Rollback(); // Rollback transaction on failure
                throw;
            }
            
        }
        else
        {
            invocation.Proceed();
        }
    }

    private void HandleAsync(IInvocation invocation)
    {
        invocation.Proceed(); // Proceed with method execution

        var returnType = invocation.Method.ReturnType;
        bool hasTransactionAttribute = hasTransactionalAttribute(invocation);

        if (returnType == typeof(Task))
        {
            invocation.ReturnValue = InterceptAsync((Task)invocation.ReturnValue, hasTransactionAttribute);
        }
        else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var resultType = returnType.GetGenericArguments()[0];
            var method = typeof(TransactionInterceptor).GetMethod(nameof(InterceptAsyncWithResult),
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var genericMethod = method.MakeGenericMethod(resultType);
            invocation.ReturnValue = genericMethod.Invoke(this, new[] { invocation.ReturnValue, hasTransactionAttribute });
        }
    }

    private async Task InterceptAsync(Task task, bool hasTransactionalAttribute)
    {
        if (hasTransactionalAttribute && _dbContext.Database.CurrentTransaction is null)
        {
            _logger.LogInformation("Starting transaction for method {0} async", task.GetType().Name);
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await task; // Await the original task
                _logger.LogInformation("Commit transaction for method {0} async", task.GetType().Name);
                await transaction.CommitAsync(); // Commit transaction on success
            }
            catch
            {
                _logger.LogInformation("Rollback transaction for method {0} async", task.GetType().Name);
                await transaction.RollbackAsync(); // Rollback transaction on failure
                throw;
            }
        }
        else
        {
            await task;
        }
    }

    private async Task<T> InterceptAsyncWithResult<T>(Task<T> task, bool hasTransactionalAttribute)
    {
        if (hasTransactionalAttribute && _dbContext.Database.CurrentTransaction is null)
        {
            _logger.LogInformation("Starting transaction for method {0} async", task.GetType().Name);
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var result = await task; // Await the original task
                _logger.LogInformation("Commit transaction for method {0} async", task.GetType().Name);
                await transaction.CommitAsync(); // Commit transaction on success
                return result; // Return the result
            }
            catch
            {
                _logger.LogInformation("Rollback transaction for method {0} async", task.GetType().Name);
                await transaction.RollbackAsync(); // Rollback transaction on failure
                throw;
            }
        }
        else
        {
            return await task;
        }
    }
}
