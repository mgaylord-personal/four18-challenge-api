namespace Four18.Common.Result;

/// <summary>
/// Result type indicating a good result with additional data of type T
/// </summary>
/// <typeparam name="T">Type of result to return</typeparam>
public class ResultOk<T> : ResultBase<T>, IResultOkId where T : class
{
    public ResultOk()
    {
        // Do nothing.
    }

    public ResultOk(T result) : base(result)
    {
        // Do nothing.
    }
}