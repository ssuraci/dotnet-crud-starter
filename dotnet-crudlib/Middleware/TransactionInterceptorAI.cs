
using AspectInjector.Broker;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace NetCrudStarter.Middleware
{
    [Aspect(Scope.PerInstance)]
    [Injection(typeof(TransactionAspect))]
    public class TransactionAspect: Attribute
    {
        private readonly ILogger<TransactionAspect> _logger;
        private readonly IServiceProvider _serviceProvider;
        public TransactionAspect()
        {
            _logger = ServiceLocator.GetService<ILogger<TransactionAspect>>();
            _serviceProvider = ServiceLocator.GetService<IServiceProvider>();
        }

        [Advice(Kind.Around, Targets = Target.Method)]
        public object HandleMethod(
            [Argument(Source.Target)] Func<object[], object> method,
            [Argument(Source.Arguments)] object[] args,
            [Argument(Source.Name)] string methodName,
            [Argument(Source.ReturnType)] Type returnType)
        {
            if (returnType == typeof(Task))
            {
               return HandleAsync(method, args, methodName, returnType);
            }
            else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var resultType = returnType.GetGenericArguments()[0];
                var m = typeof(TransactionAspect).GetMethod(nameof(HandleAsyncWithResult),
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var genericMethod = m.MakeGenericMethod(resultType);
                return genericMethod.Invoke(this, new object[] { method, args, methodName, returnType });
            }
            else
            {
                return HandleSync(method, args, methodName);
            }

            return null;
        }

        private object HandleSync(Func<object[], object> method, object[] args, string methodName)
        {
            using var scope = _serviceProvider.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            if (_dbContext.Database.CurrentTransaction is null)
            {
                _logger.LogInformation("Starting transaction for method {0} sync", methodName);
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    var result = method(args);
                    _logger.LogInformation("Commit transaction for method {0} sync", methodName);
                    transaction.Commit();
                    return result;
                }
                catch
                {
                    _logger.LogInformation("Rollback transaction for method {0} sync", methodName);
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                return method(args);
            }
        }

        private async Task<T> HandleAsyncWithResult<T>(Func<object[], object> method, object[] args, string methodName, Type returnType)
        {
            var task = (Task<T>)method(args);
            using var scope = _serviceProvider.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            T res = default(T);
            if (_dbContext.Database.CurrentTransaction is null)
            {
                _logger.LogInformation("Starting transaction for method {0} async", methodName);
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                try
                {
                    res = await task;
                    _logger.LogInformation("Commit transaction for method {0} async", methodName);
                    await transaction.CommitAsync();
                }
                catch
                {
                    _logger.LogInformation("Rollback transaction for method {0} async", methodName);
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                res = await task;
            }

            return res;
        }
        
        private async Task HandleAsync(Func<object[], object> method, object[] args, string methodName, Type returnType)
        {
            
            var task = (Task)method(args);
            using var scope = _serviceProvider.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            if (_dbContext.Database.CurrentTransaction is null)
            {
                _logger.LogInformation("Starting transaction for method {0} async", methodName);
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                try
                {
                    await task;
                    _logger.LogInformation("Commit transaction for method {0} async", methodName);
                    await transaction.CommitAsync();
                }
                catch
                {
                    _logger.LogInformation("Rollback transaction for method {0} async", methodName);
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                await task;
            }
        }

        
    }
}