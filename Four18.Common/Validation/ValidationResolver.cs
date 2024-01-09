using System.Linq;
using Four18.Common.Result;

namespace Four18.Common.Validation;

public static class ValidationResolver
{
    public static IResult GetResult(IValidationResult validationResult)
    {
        //not applicable of valid
        if (validationResult.IsValid())
        {
            return new ResultOk();
        }

        //handle not found
        if (validationResult.Errors.Any(x => x.ValidationMessage != null && x.ValidationMessage.Contains(ValidatorConstants.ResultHintForbidden)))
        {
            return new ResultForbidden
            {
                Result = CleanResultHints(validationResult)
            };
        }

        //handle not found
        if (validationResult.Errors.Any(x => x.ValidationMessage != null && x.ValidationMessage.Contains(ValidatorConstants.ResultHintNotFound)))
        {
            return new ResultNotFound();
        }

        //handle conflicts - duplicate Name, etc.
        if (validationResult.Errors.Any(x => x.ValidationMessage != null && x.ValidationMessage.Contains(ValidatorConstants.ResultHintConflict)))
        {
            return new ResultConflict
            {
                Result = CleanResultHints(validationResult)
            };
        }

        //other validations - 
        return new ResultValidation(validationResult);
    }

    private static IValidationResult CleanResultHints(IValidationResult validationResult)
    {
        var cleaned =
            from e in validationResult.Errors
            where e.ValidationMessage != null && !e.ValidationMessage.Contains(ValidatorConstants.ResultHintBase)
            select e;

        return new ValidationResult
        {
            Errors = cleaned
        };
    }
}