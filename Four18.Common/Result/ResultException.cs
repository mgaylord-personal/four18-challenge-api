using System;

namespace Four18.Common.Result;

/// <summary>
/// Result type indicating an exception has occurred
/// </summary>
public class ResultException : ResultBase<Exception>, IResultException
{
    public ResultException()
    {
        // Do nothing.
    }

    public ResultException(Exception ex) : base(ex)
    {
        // Do nothing.
    }
}