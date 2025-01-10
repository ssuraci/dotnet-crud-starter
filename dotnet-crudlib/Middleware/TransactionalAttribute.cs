//filter factory is used in order to create new filter instance per request
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCrudLib.Middleware
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TransactionalAttribute : Attribute
    {
    }
}