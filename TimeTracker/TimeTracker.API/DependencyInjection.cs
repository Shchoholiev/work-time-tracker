using Serilog;
using TimeTracker.API.ExceptionHandling;

namespace TimeTracker.API
{
    public static class DependencyInjection
    {
        public static ILoggingBuilder AddLogger(this ILoggingBuilder logging, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                             .ReadFrom.Configuration(configuration)
                             .Enrich.FromLogContext()
                             .CreateLogger();
            logging.ClearProviders();
            logging.AddSerilog(logger);

            return logging;
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
