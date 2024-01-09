namespace Four18.Common.Validation;

/// <summary>
/// Interface that can be added to a class to add validatability.
/// Wrapper around ValidationResults.
/// </summary>
public interface IValidatable
{
    /// <summary>
    /// Evaluates if ValidationResults contains errors
    /// </summary>
    /// <returns>False when Errors exist, True otherwise</returns>
    bool IsValid();

    /// <summary>
    /// Represents a set of validation results
    /// </summary>
    IValidationResult ValidationResults { get; }
}