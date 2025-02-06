namespace NetCrudStarter.Middleware;

public static class ServiceLocator
{
    public static IServiceProvider? Provider { get; set; }

    public static T GetService<T>() where T : class
    {
        return Provider?.GetService<T>() ?? throw new InvalidOperationException($"Service {typeof(T).Name} not found.");
    }
}