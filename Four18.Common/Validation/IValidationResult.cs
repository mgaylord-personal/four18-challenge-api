using System.Collections.Generic;

namespace Four18.Common.Validation;

/// <summary>
/// Represents a set of validation results
/// </summary>
public interface IValidationResult
{
    /// <summary>
    /// Evaluates if errors exist
    /// </summary>
    /// <returns>False when Errors exist, True otherwise</returns>
    bool IsValid();

    /// <summary>
    /// List of Validation failures
    /// </summary>
    IEnumerable<IValidationFailure> Errors { get; }
}