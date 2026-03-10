using FileStorageClient.Models;
using FileInfo = FileStorageClient.Models.FileInfo;

namespace FileStorageClient.Abstractions;

public interface IFileStorageClient
{
    Task<FileResult> DownloadFileAsync(
        FileInfo fileInfo,
        CancellationToken ct = default);
    
    Task UploadFileAsync(
        FileInfo fileInfo,
        Stream fileStream,
        CancellationToken ct = default);  
}