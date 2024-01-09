using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Four18.Common.Configuration;

/// <summary>
/// Serilog blob storage sink configuration options.
/// </summary>
public class SerilogBlobStorageConfigurationOptions
{
    /// <summary>
    /// blob storage connection string.
    /// </summary>
    public string? ConnectionString { get; set; }
        
    /// <summary>
    /// blob storage container name.
    /// </summary>
    public string? ContainerName { get; set; }
        
    /// <summary>
    /// Filename for log file - supports date formatting {{dd} {MM}} etc.
    /// </summary>
    public string? FileName { get; set; }
        
    /// <summary>
    /// Minimum logging event level.
    /// </summary>
    public LogEventLevel MinimumLevel { get; set; }

    /// <summary>
    /// Write separate log events batched together to blob storage.
    /// </summary>
    public bool WriteInBatches { get; set; }
        
    /// <summary>
    /// Period in which to queue all log events before sending to blob storage.
    /// </summary>
    public TimeSpan Period { get; set; }

    /// <summary>
    /// Max number of event logs to be batched together before sending to blob storage.
    /// </summary>
    public int? BatchPostingLimit { get; set; }
}

/// <summary>
/// Extension methods for <see cref="SerilogBlobStorageConfigurationOptions"/>.
/// </summary>
public static class SerilogBlobStorageConfigurationOptionsExtensions
{
    /// <summary>
    /// Configures Serilog blob storage sink from configuration using <see cref="SerilogBlobStorageConfigurationOptions"/>.
    /// If <see cref="SerilogBlobStorageConfigurationOptions.ConnectionString"/> or <see cref="SerilogBlobStorageConfigurationOptions.ContainerName"/>
    /// or <see cref="SerilogBlobStorageConfigurationOptions.FileName"/> are not specified the sink will not be enabled.
    /// </summary>
    public static LoggerConfiguration WriteToBlobStorage(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        var serilogBlobStorageOptions = configuration.GetSection("SerilogBlobStorage").Get<SerilogBlobStorageConfigurationOptions>();
        if (serilogBlobStorageOptions != null && !string.IsNullOrEmpty(serilogBlobStorageOptions.ConnectionString)
                                                   && !string.IsNullOrEmpty(serilogBlobStorageOptions.ContainerName)
                                                   && !string.IsNullOrEmpty(serilogBlobStorageOptions.FileName))
        {
            var connectionString = serilogBlobStorageOptions.ConnectionString.Trim();
            var containerName = serilogBlobStorageOptions.ContainerName.Trim();
#pragma warning disable CA1305
            //loggerConfiguration.WriteTo.AzureBlobStorage(connectionString, serilogBlobStorageOptions.MinimumLevel, containerName, serilogBlobStorageOptions.FileName,
            //    null, serilogBlobStorageOptions.WriteInBatches, serilogBlobStorageOptions.Period, serilogBlobStorageOptions.BatchPostingLimit);
#pragma warning restore CA1305
        }

        return loggerConfiguration;
    }
}