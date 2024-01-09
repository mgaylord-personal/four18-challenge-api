namespace Four18.Common.Result;

/// <summary>
/// Interface indicating a good result with additional data of type T
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOkResult<T> : IResult<T>, IResultOkId where T : class
{
}