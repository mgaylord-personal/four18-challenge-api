namespace Four18.Common.Validation;

/// <summary>
/// Information regarding a data validation failure
/// </summary>
public class ValidationFailure : IValidationFailure
{
    /// <summary>
    /// The value of the value attempted
    /// </summary>
    public object? AttemptedValue { get; set; }

    /// <summary>
    /// State related information - typically blank
    /// </summary>
    public object? CustomState { get; set; }

    /// <summary>
    /// Error message produced by the validation logic
    /// </summary>
    public string? ValidationMessage { get; set; }

    /// <summary>
    /// Property being validated
    /// </summary>
    public string? PropertyName { get; set; }
}