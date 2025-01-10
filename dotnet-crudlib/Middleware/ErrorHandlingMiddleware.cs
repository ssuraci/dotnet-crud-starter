using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using WebapiTemplate.WebapiException;

namespace WebapiTemplate.Filter
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

       private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>();
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            string msg = exception.Message;
            if (exception is AppException)
            {
                code = HttpStatusCode.BadRequest;
                AppException appException = (AppException)exception;
                if (appException.InnerException != null) {
                    if (appException.InnerException.InnerException != null)
                    {
                        msg = appException.InnerException.InnerException.Message;
                    }
                }
                if (msg == null || msg.StartsWith("WebapiTemplate."))
                {
                    msg = "Errore interno";
                }
            }
            _logger.LogError(exception, "exception");
            var result = JsonConvert.SerializeObject(msg);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
