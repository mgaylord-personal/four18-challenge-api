using System.Collections.Generic;
using System.Linq;

namespace Four18.Common.Validation;

/// <summary>
/// Represents a set of validation results
/// </summary>
public class ValidationResult : IValidationResult
{
    public ValidationResult()
    {
        Errors = new List<ValidationFailure>();
    }

    /// <summary>
    /// Evaluates if errors exist
    /// </summary>
    /// <returns>False when Errors exist, True otherwise</returns>
    public bool IsValid()
    {
        return !Errors.Any();
    }

    /// <summary>
    /// List of Validation failures
    /// </summary>
    public IEnumerable<IValidationFailure> Errors { get; set; }
}