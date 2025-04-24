using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MusiciansAPP.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder SetUpLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Services.AddLogging(loggingBuilder =>
          loggingBuilder.AddSerilog(dispose: true));

        return builder;
    }
}