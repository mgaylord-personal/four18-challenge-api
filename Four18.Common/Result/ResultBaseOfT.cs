namespace Four18.Common.Result;

/// <summary>
/// Result base class for data being returned
/// </summary>
/// <typeparam name="T">Type of data being returned</typeparam>
public abstract class ResultBase<T> : IResult<T> where T : class
{
    /// <summary>
    /// The data being returned
    /// </summary>
    public T Result { get; set; }

    protected ResultBase()
    {
        // Do nothing.
    }

    protected ResultBase(T result)
    {
        Result = result;
    }
}