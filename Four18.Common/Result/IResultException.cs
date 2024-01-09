using System;

namespace Four18.Common.Result;

/// <summary>
/// Interface for Result type indicating an exception has occurred
/// </summary>
public interface IResultException : IResult<Exception>
{
}