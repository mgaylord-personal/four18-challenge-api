using System.Text.RegularExpressions;

namespace Four18.Common.Logging;

/// <summary>
/// Logging constants
/// </summary>
public static partial class LoggingConstants
{
    public const string LogClass = "LogClass";
    public const string LogMethod = "LogMethod";
    public const string LogClassMethod = LogClass + ":{" + LogClass + "}, " + LogMethod + ": {" + LogMethod + "}";

    /// <summary>
    /// Regex pattern used to omit specific error messages from logging
    /// </summary>
    [GeneratedRegex("IDX(10634|10205)", RegexOptions.IgnoreCase, "en-US")]
    public static partial Regex LogMessagesToOmitRegex();
}
