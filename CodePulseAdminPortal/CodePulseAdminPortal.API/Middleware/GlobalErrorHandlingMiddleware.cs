using CodePulseAdminPortal.API.Common.Exceptions;
using System.Net;
using NotImplementedException= CodePulseAdminPortal.API.Common.Exceptions.NotImplementedException;
using UnauthorizedAccessException = CodePulseAdminPortal.API.Common.Exceptions.UnauthorizedAccessException;
using KeyNotFoundException = CodePulseAdminPortal.API.Common.Exceptions.KeyNotFoundException;
using System.Text.Json;

namespace CodePulseAdminPortal.API.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next,ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public  async Task InvokeAsync(HttpContext context)
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
            HttpStatusCode status;
            var stackTrace = string.Empty;
            string message;
            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadHttpRequestException))
            {
                status=HttpStatusCode.BadRequest;
                message=exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                status = HttpStatusCode.NotImplemented;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                status = HttpStatusCode.NotFound;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                status = HttpStatusCode.NotFound;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else 
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            var exceptionResult= JsonSerializer.Serialize(new 
            { 
                Exceptionmessage = message,
                ExceptionStackTrace = stackTrace, 
                ExceptionStatusCode= status
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            logger.LogError(exception, exception.Message);
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
