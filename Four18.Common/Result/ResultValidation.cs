using System.Collections.Generic;
using Four18.Common.Validation;

namespace Four18.Common.Result;

/// <summary>
/// Result type for validation errors
/// </summary>
public class ResultValidation : ResultBase<IValidationResult>, IResultValidation
{
    /// <summary>
    /// constructor
    /// </summary>
    public ResultValidation()
    {
        Result = new ValidationResult();
    }

    public ResultValidation(IValidationResult result) : base(result)
    {
        // Do nothing.
    }

    /// <summary>
    /// constructor
    /// </summary>
    public ResultValidation(IValidationFailure validationFailure )
    {
        Result = new ValidationResult
        {
            Errors = new List<IValidationFailure>
            {
                validationFailure
            }
        };
    }
}