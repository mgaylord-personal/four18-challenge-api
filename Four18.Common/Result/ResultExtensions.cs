using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Four18.Common.Result;

public static class ResultExtensions
{
    /// <summary>
    /// Unwraps a typed IResult with its value; a null response indicates a failed result.
    /// Optionally log a failed result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="logger"></param>
    /// <typeparam name="T">Type of IResult</typeparam>
    /// <exception cref="InvalidOperationException">Thrown when unwrapping an "okay-type" untyped IResult such as ResultOk or ResultFileOk</exception>
    /// <returns>Typed IResult or null if the result errored</returns>
    public static T? UnwrapTypedResult<T>(this IResult result, ILogger? logger = null) where T : class
    {
        switch (result)
        {
            case IResult<T> typedResultOk:
                return typedResultOk.Result;
            case IResultOk:
                throw new InvalidOperationException("Attempted to unwrap a typed IResult that contains no type");
            case IResultException resultException:
                logger?.LogError(resultException.Result.Message);
                if (resultException.Result.InnerException?.Message != null)
                {
                    logger?.LogError(resultException.Result.InnerException.Message);
                }
                break;
            case IResultValidation resultValidation:
                var validationMessage = resultValidation.Result.Errors
                    .Select(e => e.ValidationMessage)
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Aggregate((c, n) => $"{c}, {n}");
                logger?.LogError(validationMessage);
                break;
            case IResultNotFound:
                logger?.LogError("Unwraped result with expected type {0} returned ResultNotFound.", nameof(T));
                break;
            case IResultUnknownId:
                logger?.LogError("Unwraped result with expected type {0} returned ResultUnknownId.", nameof(T));
                break;
        }

        return null;
    }
    
    /// <summary>
    /// Unwraps a untyped IResult and returns if the result is successful.
    /// Optionally log a failed result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="logger"></param>
    /// <exception cref="InvalidOperationException">Thrown when unwrapping a typed IResult</exception>
    /// <returns>Success status of IResult</returns>
    public static bool UnwrapUntypedResult(this IResult result, ILogger? logger = null)
    {
        switch (result)
        {
            case IResultOk:
                return true;
            case IResultOkId:
                throw new InvalidOperationException("Attempted to unwrap an untyped IResult that contains a typed result");
            case IResultException resultException:
                logger?.LogError(resultException.Result.Message);
                if (resultException.Result.InnerException?.Message != null)
                {
                    logger?.LogError(resultException.Result.InnerException.Message);
                }
                break;
            case IResultValidation resultValidation:
                var validationMessage = resultValidation.Result.Errors
                    .Select(e => e.ValidationMessage)
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Aggregate((c, n) => $"{c}, {n}");
                logger?.LogError(validationMessage);
                break;
            case IResultNotFound:
                logger?.LogError("Unwraped untyped result returned ResultNotFound.");
                break;
            case IResultUnknownId:
                logger?.LogError("Unwraped untyped result returned ResultUnknownId.");
                break;
        }

        return false;
    }
}
