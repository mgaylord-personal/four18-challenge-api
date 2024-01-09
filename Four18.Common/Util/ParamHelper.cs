namespace Four18.Common.Util;

public static class ParamHelper
{
    public static string VOrNull(string v)
    {
        var t = v?.Trim();
        return string.IsNullOrEmpty(t) || string.IsNullOrWhiteSpace(t)
            ? "null"
            : "'" + t + "'";
    }
}