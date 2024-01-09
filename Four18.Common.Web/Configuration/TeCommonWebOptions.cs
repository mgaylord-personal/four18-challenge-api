using System;
using Microsoft.Extensions.DependencyInjection;

namespace Four18.Common.Web.Configuration;

public class TeCommonWebOptions
{
    /// <summary>
    /// Use Newtonsoft Json serialization, if false uses System.Text.Json
    /// </summary>
    public bool UseNewtonsoftJson { get; set; } = true;

    /// <summary>
    /// Add custom health check(s) to the default health check endpoint through a callback method
    /// </summary>
    public Action<IHealthChecksBuilder>? CustomHealthChecks { get; set; }
}
