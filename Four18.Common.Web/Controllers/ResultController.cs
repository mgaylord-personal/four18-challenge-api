using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Four18.Common.Result;

namespace Four18.Common.Web.Controllers;

/// <inheritdoc />
/// <summary>
/// Base Controller implementing common functionality for Claims and Responses
/// </summary>
[Authorize]
public abstract class ResultController : Controller
{
    protected ILogger<ResultController> Logger { get; }

    protected ResultController(ILogger<ResultController> logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Converts and IResult type to an IActionResult type
    /// </summary>
    /// <param name="result">IResult type being evaluated</param>
    /// <returns>IActionResult with optional response data</returns>
    protected virtual IActionResult ResolveResult(IResult result)
    {
        try
        {
            return result switch
            {
                IResultException exRes => HandleExceptionResult(exRes),
                IResultForbidden forRes => HandleForbiddenResult(forRes),
                IResultConflict conRes => HandleConflictResult(conRes),
                IResultValidation valRes => HandleValidationResult(valRes),
                IResultNotFound _ => HandleNotFoundResult(result),
                IResultCreatedId creRes => HandleCreatedResult(creRes),
                IResultOkId oktRes => HandleOkOfTResult(oktRes),
                IResultFileOk okfRes => HandleFileOkResult(okfRes),
                IResultOk okRes => HandleOkResult(okRes),
                _ => HandleOtherResult(result)
            };
        }
        catch (Exception exception)
        {
            Logger.LogError(exception.Message);
            return new ObjectResult(exception)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                DeclaredType = exception.GetType(),
                Value = new { Result = "An unknown error has occurred" }
            };
        }
    }

    /// <summary>
    /// Catch-all to return a NotFoundResult
    /// </summary>
    /// <param name="result">Any IResult type</param>
    /// <returns>Http Status Code 404 - NotFoundResult</returns>
    protected virtual IActionResult HandleNotFoundResult(IResult result)
    {
        Logger.LogWarning("Controller: {cont}, Method: {meth}, Not Found Result",
            nameof(ResultController),
            nameof(HandleNotFoundResult));
        return NotFound();
    }

    /// <summary>
    /// Converts IResultException to InternalServerError
    /// </summary>
    /// <param name="result">IResultException Type</param>
    /// <returns>Http Status Code 500 - Internal Server Error</returns>
    protected virtual IActionResult HandleExceptionResult(IResultException result)
    {
        Logger.LogError(0, result.Result, result.Result?.Message);
        Logger.LogError("Controller: {cont}, Method: {meth}, Exception Result",
            nameof(ResultController),
            nameof(HandleExceptionResult));
        return new ObjectResult(result)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            DeclaredType = result.Result?.GetType(),
            Value = result.Result?.Message
        };
    }

    /// <summary>
    /// Converts an IResultForbidden to a Forbidden
    /// </summary>
    /// <param name="result">IResultForbidden</param>
    /// <returns>Http Status Code 403 - Forbidden, with validation error list</returns>
    protected virtual IActionResult HandleForbiddenResult(IResultForbidden result)
    {
        Logger.LogWarning("Controller: {cont}, Method: {meth}, Forbidden Result",
            nameof(ResultController),
            nameof(HandleForbiddenResult));
        return new ForbidResult();
    }

    /// <summary>
    /// Converts an IResultConflict to a Conflict
    /// </summary>
    /// <param name="result">IResultConflict</param>
    /// <returns>Http Status Code 409 - Conflict, with validation error list</returns>
    protected virtual IActionResult HandleConflictResult(IResultConflict result)
    {
        Logger.LogWarning("Controller: {cont}, Method: {meth}, Conflict Result : {@validationResults}",
            nameof(ResultController),
            nameof(HandleConflictResult), result);
        return new ObjectResult(result)
        {
            StatusCode = (int)HttpStatusCode.Conflict,
            DeclaredType = result.Result?.GetType(),
            Value = result.Result
        };
    }

    /// <summary>
    /// Converts an IResultValidation to a Bad Request
    /// </summary>
    /// <param name="result">IResultValidation</param>
    /// <returns>Http Status Code 400 - Bad Request, with validation error list</returns>
    protected virtual IActionResult HandleValidationResult(IResultValidation result)
    {
        Logger.LogWarning("Controller: {cont}, Method: {meth}, Validation Error Result : {@validationResults}",
            nameof(ResultController),
            nameof(HandleValidationResult), result);
        return new ObjectResult(result)
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            DeclaredType = result.Result?.GetType(),
            Value = result.Result
        };
    }

    /// <summary>
    /// Converts an IResultCreated to a HttpStatusCode.Created
    /// </summary>
    /// <param name="result">IResultCreatedId (ResultCreated of Type)</param>
    /// <returns>Http Status Code 201 - Created, with data</returns>
    protected virtual IActionResult HandleCreatedResult(IResultCreatedId result)
    {
        Logger.LogInformation("Controller: {cont}, Method: {meth}, Created Result",
            nameof(ResultController),
            nameof(HandleCreatedResult));
        return new CreatedResult(Request.Path, result);
    }

    /// <summary>
    /// Converts IResultOkId to Ok result
    /// </summary>
    /// <param name="result">IResultOkId (ResultOk of Type)</param>
    /// <returns>Http Status Code 200 - Ok, with data</returns>
    protected virtual IActionResult HandleOkOfTResult(IResultOkId result)
    {
        var r = Ok(result);
        Logger.LogInformation("Controller: {cont}, Method: {meth}",
            nameof(ResultController),
            nameof(HandleOkOfTResult));
        return r;
    }

    /// <summary>
    /// Converts IResultOkId to Ok result
    /// </summary>
    /// <param name="result">IResultOkId (ResultOk of Type)</param>
    /// <returns>Http Status Code 200 - Ok, with data</returns>
    protected virtual IActionResult HandleFileOkResult(IResultFileOk result)
    {
        Logger.LogInformation("Controller: {cont}, Method: {meth}, FileName: {file}",
            nameof(ResultController),
            nameof(HandleFileOkResult),
            result.FileName);
#pragma warning disable CS8604 // Possible null reference argument.
        return File(result.File, result.ContentType, result.FileName);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    /// <summary>
    /// Converts IResultOk to Ok result
    /// </summary>
    /// <param name="result">IResultOk (no data)</param>
    /// <returns>Http Status Code 200 - Ok, no data</returns>
    protected virtual IActionResult HandleOkResult(IResultOk result)
    {
        Logger.LogInformation("Controller: {cont}, Method: {meth}, Ok Result",
            nameof(ResultController),
            nameof(HandleOkResult));
        return Ok(result);
    }

    /// <summary>
    /// Generic catch-all for Bad Request
    /// </summary>
    /// <param name="result">Any IResult type</param>
    /// <returns>Http Status Code 400 - Bad request, with message</returns>
    protected virtual IActionResult HandleOtherResult(IResult result)
    {
        Logger.LogWarning("Controller: {cont}, Method: {meth}, Unable to resolve result.",
            nameof(ResultController),
            nameof(HandleOtherResult));
        return BadRequest("Unable to resolve result.");
    }

}