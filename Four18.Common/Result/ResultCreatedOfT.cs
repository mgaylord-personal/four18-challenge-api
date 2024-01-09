namespace Four18.Common.Result;

/// <summary>
/// Result type indicating data is unknown
/// </summary>
/// <typeparam name="T">The Type of the data being returned</typeparam>
public class ResultCreated<T> : ResultBase<T>, IResultCreated<T> where T : class
{
    public ResultCreated()
    {
        // Do nothing.
    }

    public ResultCreated(T result) : base(result)
    {
        // Do nothing.
    }
}