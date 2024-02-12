using Microsoft.AspNetCore.StaticFiles;

namespace WebApi.Utils;

public static class FileHelper
{
    public static string GetMimeType(string fileName)
    {

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out string? contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }

    public static string FileNameWithExt(string fileName, string dirFull, string idFileName)
    {
        var extPosition = fileName.LastIndexOf('.');
        var ext = extPosition >= 0 ? fileName.Substring(extPosition) : "";
        return Path.Combine(dirFull, dirFull, $"{idFileName}{ext}");
    }

}