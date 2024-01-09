namespace Four18.Common.Validation;

public static class ValidatorConstants
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    #region Rulesets - Standard

    public const string RuleSetAdd = "Add";
    public const string RuleSetAlways = "Always";
    public const string RuleSetDelete = "Delete";
    public const string RuleSetGet = "Get";
    public const string RuleSetUpdate = "Update";
    public const string RuleSetUpdateDeleteCommon = "UpdateDeleteCommon";
    public const string RuleSetAddDefault = $"{RuleSetAdd},{RuleSetAlways}";
    public const string RuleSetGetDefault = $"{RuleSetGet},{RuleSetAlways}";
    public const string RuleSetUpdateDefault = $"{RuleSetUpdate},{RuleSetAlways},{RuleSetUpdateDeleteCommon}";
    public const string RuleSetDeleteDefault = $"{RuleSetDelete},{RuleSetAlways},{RuleSetUpdateDeleteCommon}";

    public static string[] RuleSetAddDefaultParams = {RuleSetAdd, RuleSetAlways};
    public static string[] RuleSetGetDefaultParams = {RuleSetGet, RuleSetAlways};
    public static string[] RuleSetUpdateDefaultParams = {RuleSetUpdate, RuleSetAlways, RuleSetUpdateDeleteCommon};
    public static string[] RuleSetDeleteDefaultParams = {RuleSetDelete, RuleSetAlways, RuleSetUpdateDeleteCommon};

    #endregion

    #region ResultHints

    public const string ResultHintBase = "[[ResultHint]]";
    public const string ResultHintConflict = ResultHintBase + "Conflict";
    public const string ResultHintForbidden = ResultHintBase + "Forbidden";
    public const string ResultHintNotFound = ResultHintBase + "NotFound";

    #endregion

#pragma warning restore CA2211 // Non-constant fields should not be visible
}