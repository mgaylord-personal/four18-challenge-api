using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Four18.Common.Util;

public static class FileHelper
{
    private static readonly Regex FileNameSanitizeFirstStageRegex = new Regex(@"[^a-zA-Z0-9\(\)\[\]_\-\%\$\~\'\`\&\@\# ]+", RegexOptions.Singleline | RegexOptions.Compiled);
    private const string FileNameSanitizeFirstStageReplacement = "¦";
    private static readonly Regex FileNameSanitizeSecondStageRegex = new Regex($@"[{FileNameSanitizeFirstStageReplacement}]+", RegexOptions.Singleline | RegexOptions.Compiled);
    private const string FileNameSanitizeSecondStageReplacement = "_";
        
    /// <summary>
    /// Sanitizes a file extension. If the file extension is provided with a delimiter, the delimiter will be removed on output
    /// </summary>
    public static string SanitizeFileExtension(string? fileExtension, string emptyExtensionReplacement = "txt")
    {
        // normalize if null
        fileExtension ??= string.Empty;
            
        var sanitizedFileExtension = SanitizeFileNamePart((fileExtension).Trim('.').Trim());
        if (string.IsNullOrWhiteSpace(sanitizedFileExtension))
        {
            sanitizedFileExtension = emptyExtensionReplacement;
        }
#pragma warning disable CA1308 // Normalize strings to uppercase
        return sanitizedFileExtension.ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
    }
        
    /// <summary>
    /// Sanitizes a file name, making it safe to use. The file name parameter should not contain directory information.
    /// </summary>
    public static string SanitizeFileName(string? fileName, string emptyFileNameReplacement = "file", string emptyFileExtensionReplacement = "dat")
    {
        // normalize if null
        fileName ??= string.Empty;

        // find the file extension if any
        string fileNameWithoutExtension;
        string fileExtension;

        var extensionSeparatorCharIndex = fileName.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
        if (extensionSeparatorCharIndex > 0)
        {
            fileNameWithoutExtension = fileName.Substring(0, extensionSeparatorCharIndex);
            var fileExtensionLength = fileName.Length - extensionSeparatorCharIndex - 1;
            fileExtension = fileName.Substring(extensionSeparatorCharIndex + 1, fileExtensionLength);
        }
        else
        {
            // no extension or the extension delimiter was found on the first position
            fileNameWithoutExtension = fileName;
            fileExtension = string.Empty;
        }

        var sanitizedFileNameWithoutExtension = SanitizeFileNameWithoutExtension(fileNameWithoutExtension, emptyFileNameReplacement);
        var sanitizedFileExtension = SanitizeFileExtension(fileExtension, emptyFileExtensionReplacement);
        var sanitizedFileName = Path.ChangeExtension(sanitizedFileNameWithoutExtension, sanitizedFileExtension);
        return sanitizedFileName;
    }
        
    /// <summary>
    /// Sanitizes a file name, making it safe to use. The file name parameter should not contain directory information.
    /// </summary>
    private static string SanitizeFileNamePart(string fileNamePart)
    {
        fileNamePart = FileNameSanitizeSecondStageRegex.Replace(
            FileNameSanitizeFirstStageRegex.Replace(fileNamePart.Trim(), FileNameSanitizeFirstStageReplacement),
            FileNameSanitizeSecondStageReplacement);

        return fileNamePart;
    }
        
    /// <summary>
    /// Sanitizes a file name without extension
    /// </summary>
    public static string SanitizeFileNameWithoutExtension(string? fileNameWithoutExtension, string emptyFileNameReplacement = "file")
    {
        // normalize if null
        fileNameWithoutExtension ??= string.Empty;
            
        var sanitizedFileName = SanitizeFileNamePart((fileNameWithoutExtension).Trim());
        if (string.IsNullOrWhiteSpace(sanitizedFileName))
        {
            sanitizedFileName = emptyFileNameReplacement;
        }
        return sanitizedFileName;
    }
}