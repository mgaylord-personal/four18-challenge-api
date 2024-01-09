using System.IO;

namespace Four18.Common.Result;

public interface IResultFileOk: IResultOk
{
    Stream? File { get; set; }
    string? ContentType { get; set; }
    string? FileName { get; set; }
}