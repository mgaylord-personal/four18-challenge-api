namespace Four18.Common.Result;

/// <summary>
/// Result type indicating data is unknown
/// </summary>
/// <typeparam name="T">The Type of the data being returned</typeparam>
public class ResultUnknown<T> : ResultBase<T>, IResultUnknown<T> where T : class
{
    public ResultUnknown()
    {
        // Do nothing.
    }

    public ResultUnknown(T result) : base(result)
    {
        // Do nothing.
    }
}