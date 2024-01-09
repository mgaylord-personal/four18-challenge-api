namespace Four18.Common.Result;

/// <summary>
/// Interface represent a result that has data
/// </summary>
/// <typeparam name="T">The type of the data being returned</typeparam>
public interface IResult<T> : IResult where T : class
{
    /// <summary>
    /// The data being returned by the result
    /// </summary>
    T Result { get; set; }
}