using System.IO;

namespace Four18.Common.Result;

public class ResultFileOk : IResultFileOk
{
    public Stream? File { get; set; }
    public string? ContentType { get; set; }
    public string? FileName { get; set; }
}