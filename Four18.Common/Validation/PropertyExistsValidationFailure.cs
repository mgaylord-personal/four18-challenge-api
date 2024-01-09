namespace Four18.Common.Validation;

/// <summary>
/// Information regarding a data validation failure
/// </summary>
public class PropertyExistsValidationFailure : ValidationFailure
{
    public PropertyExistsValidationFailure()
    {
        // Do nothing.
    }

    public PropertyExistsValidationFailure(string propertyName)
    {
        PropertyName = propertyName;
        ValidationMessage = $"Property '{propertyName}' already exists.";
    }
}