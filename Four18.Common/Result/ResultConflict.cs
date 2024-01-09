using Four18.Common.Validation;

namespace Four18.Common.Result;

/// <summary>
/// Result type for validation errors which are conflicts
/// </summary>
public class ResultConflict : ResultValidation, IResultConflict
{
    public ResultConflict()
    {
        // Do nothing.
    }

#pragma warning disable CA1801 // Review unused parameters
    public ResultConflict(string propertyName) : base(new PropertyExistsValidationFailure())
#pragma warning restore CA1801 // Review unused parameters
    {
        // Do nothing.
    }
}