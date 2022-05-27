using System.Net;

namespace TimeTracker.API.ExceptionHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this._logger = logger;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this._next(httpContext);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                this._logger.LogError($"Parameter is out of range: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            var message = exception switch
            {
                ArgumentOutOfRangeException => $"Parameter is out of range: {exception.Message}",
                _ => "Internal Server Error",
            };

            await context.Response.WriteAsync(
                new ErrorDetails(context.Response.StatusCode, message).ToString());
        }
    }
}
