namespace Four18.Common.Result;

/// <summary>
/// Interface representing return data that is unknown
/// </summary>
public interface IResultUnknown<T> : IResult<T>, IResultUnknownId where T : class
{
}