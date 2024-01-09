namespace Four18.Common.Validation;

/// <summary>
/// Information regarding a data validation failure
/// </summary>
public interface IValidationFailure
{
    /// <summary>
    /// The value of the value attempted
    /// </summary>
    object? AttemptedValue { get; set; }

    /// <summary>
    /// State related information - typically blank
    /// </summary>
    object? CustomState { get; set; }

    /// <summary>
    /// Error message produced by the validation logic
    /// </summary>
    string? ValidationMessage { get; set; }

    /// <summary>
    /// Property being validated
    /// </summary>
    string? PropertyName { get; set; }
}