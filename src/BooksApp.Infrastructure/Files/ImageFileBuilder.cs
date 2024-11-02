using BooksApp.Application.Common.Constants;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace BooksApp.Infrastructure.Files;

public class ImageFileBuilder(string path) : IImageFileBuilder
{
    public async Task<string?> CreateImage(IFormFile file, CancellationToken token = default)
    {
        var savePath = Path.Combine(path);
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(savePath, $"{fileName}");
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream, token);
        return fileName;
    }

    public async Task<bool> DeleteImage(string fileName, CancellationToken token = default)
    {
        var fileInfo = new FileInfo(
            Path.Combine(path,
                fileName
            )
        );
        var exists = fileInfo.Exists;
        if (exists) fileInfo.Delete();
        return exists;
    }

    public async Task<(FileInfo fileInfo, FileStream fileStream)> RetrieveImage(string fileName,
        CancellationToken token = default)
    {
        var uploadPath =
            Path.Combine(path);
        var filePath = Path.Combine(uploadPath, fileName);
        var fileInfo = new FileInfo(filePath);
        var fileStream = new FileStream(filePath, FileMode.Open);

        return (fileInfo, fileStream);
    }

    public bool AnyImage(string fileName, CancellationToken token = default)
    {
        var fileInfo = new FileInfo(
            Path.Combine(path,
                fileName
            )
        );
        return fileInfo.Exists;
    }

    public bool IsValid(string fileName, CancellationToken token = default)
    {
        var path = Path.Combine(fileName);
        if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) > 0) return false;
        var fileInfo = new FileInfo(path);
        var extension = fileInfo.Extension.Replace(".", "");
        return AppConstants.AllowedExtensions.Contains(extension);
    }
}