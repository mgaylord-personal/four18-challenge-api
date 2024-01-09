namespace Four18.Common.Result;

/// <summary>
/// Interface representing return data that has been created
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public interface IResultCreated<T> : IResult<T>, IResultCreatedId where T : class
{
}