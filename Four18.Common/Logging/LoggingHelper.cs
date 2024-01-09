using System.Linq;
using Newtonsoft.Json;
using Four18.Common.Util;

namespace Four18.Common.Logging;

public static class LoggingHelper
{
    /// <summary>
    /// Returns logging message in the form of 
    /// "LogClass: {LogClass}, LogMethod: {LogMethod}, Param0: {Param0}, ... "ParamX: {ParamX}"
    /// where "Param0" names are specified in additionalParams
    /// </summary>
    /// <param name="additionalParams">parameter names to include in the message</param>
    /// <returns>string</returns>
    public static string GetLogClassMethodWithParams(params string[]? additionalParams)
    {
        const string result = LoggingConstants.LogClassMethod;
        if (additionalParams == null || additionalParams.Length == 0) return result;
        return additionalParams
            .Select(x => StringHelper.FirstCharToUpper(x))
            .Aggregate(result, (current, upParam) => current + $", {upParam}: {{{upParam}}}");
    }

    /// <summary>
    /// JSON serializes the the object
    /// </summary>
    public static string JsonSerializeObject(object obj)
    {
        try
        {
            return JsonConvert.SerializeObject(obj);
        }
        catch
        {
            return "object failed to serialize";
        }
    }
}