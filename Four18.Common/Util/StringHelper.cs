using System.Globalization;
using System.Linq;

namespace Four18.Common.Util;

public static class StringHelper
{
    /// <summary>
    /// Ensures first char is uppercase
    /// </summary>
    /// <param name="input"></param>
    /// <param name="remainingToLower"></param>
    /// <returns></returns>
    public static string FirstCharToUpper(string input, bool remainingToLower = false)
    {
        if (IsInputNullOrEmpty(input)) {return input;}
        return GetFirstCharUpper(input) + SubStringAfterFirst(input, remainingToLower);
    }

    /// <summary>
    /// Returns the substring after the first character
    /// </summary>
    /// <param name="input"></param>
    /// <param name="remainingToLower">when true, will be lowercase</param>
    /// <returns></returns>
    private static string SubStringAfterFirst(string input, bool remainingToLower = false)
    {
#pragma warning disable CA1308 // Normalize strings to uppercase
        return IsInputNullOrEmpty(input)
            ? string.Empty
            : remainingToLower
                ? input.Substring(1).ToLower(CultureInfo.CurrentCulture)
                : input.Substring(1);
#pragma warning restore CA1308 // Normalize strings to uppercase
    }

    /// <summary>
    /// IsNullOrEmpty with Trim()
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static bool IsInputNullOrEmpty(string input)
    {
        return string.IsNullOrEmpty(input?.Trim());
    }

    /// <summary>
    /// Returns first char as uppercase
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static string GetFirstCharUpper(string input)
    {
        return IsInputNullOrEmpty(input)
            ? string.Empty
            : input.First().ToString().ToUpper(CultureInfo.CurrentCulture);
    }
}