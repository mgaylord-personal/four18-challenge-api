using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Four18.Common.Configuration;

/// <summary>
/// Sentry configuration options
/// </summary>
public class SentryConfigurationOptions
{
    /// <summary>
    /// Unique url identifier that enables the Sentry integration
    /// </summary>
    public string? Dsn { get; set; }
        
    /// <summary>
    /// Minimum log level to log as breadcrumb within an event
    /// </summary>
    public LogEventLevel MinimumBreadcrumbLevel { get; set; }
        
    /// <summary>
    /// Minimum log level to log as event
    /// </summary>
    public LogEventLevel MinimumEventLevel { get; set; }
        
    /// <summary>
    /// Maximum breadcrumb count within an event
    /// </summary>
    public int? MaxBreadcrumbs { get; set; }
}

/// <summary>
/// Extension methods for <see cref="SentryConfigurationOptions"/>
/// </summary>
public static class SentryConfigurationOptionsExtensions
{
    /// <summary>
    /// Configures Serilog Sentry sink from configuration using <see cref="SentryConfigurationOptions"/>.
    /// If <see cref="SentryConfigurationOptions.Dsn"/> is not specified the sink will not be enabled.
    /// </summary>
    public static LoggerConfiguration WriteToSentry(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, bool initializeSentrySdk = true)
    {
        var sentryConfigurationOptions = configuration.GetSection("Sentry").Get<SentryConfigurationOptions>();
        if (sentryConfigurationOptions != null && !string.IsNullOrEmpty(sentryConfigurationOptions.Dsn))
        {
            loggerConfiguration.WriteTo.Sentry(options =>
            {
                options.InitializeSdk = initializeSentrySdk;
                options.Dsn = sentryConfigurationOptions.Dsn;
                options.MinimumBreadcrumbLevel = sentryConfigurationOptions.MinimumBreadcrumbLevel;
                options.MinimumEventLevel = sentryConfigurationOptions.MinimumEventLevel;
                options.MaxBreadcrumbs = sentryConfigurationOptions.MaxBreadcrumbs ?? 100;
            });
        }

        return loggerConfiguration;
    }
}