using System.IO.Compression;
using System.IO.Pipelines;
using Amazon.S3;
using Amazon.S3.Transfer;
using FileStorageClient.Abstractions;
using Microsoft.Extensions.Options;

namespace FileStorageClient.S3Implementation;

internal class S3ArchivingLoader : IDisposable
{
    private readonly IAmazonS3 _client;
    private readonly string _bucketName;
    
    private bool _disposed;
    
    public S3ArchivingLoader(IS3ClientFactory s3ClientFactory, IOptions<FileStorageOptions> options)
    {
        _client = s3ClientFactory.CreateClient();
        _bucketName = options.Value.BucketName;
    }
    
    public Task UploadAsync(string archiveKey, IEnumerable<(FileInfo, Stream)> files, CancellationToken ct = default)
    {
        var pipe = new Pipe();
        var zipTask = CreateArchiveAsync(pipe.Writer, files, ct);
		
        var transferUtility = new TransferUtility(_client);
        var uploadTask = transferUtility.UploadAsync(pipe.Reader.AsStream(), _bucketName, archiveKey, ct);
        
        return Task.WhenAll(zipTask, uploadTask);
    }
	
    private static async Task CreateArchiveAsync(PipeWriter pipeWriter, IEnumerable<(FileInfo Info, Stream Content)> files, CancellationToken ct = default)
    {
        await using var zipStream = pipeWriter.AsStream();
        await using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            foreach (var file in files)
            {
                var entry = archive.CreateEntry(file.Info.Name, CompressionLevel.Optimal);
                await using var entryStream = await entry.OpenAsync(ct);
                await file.Content.CopyToAsync(entryStream, ct);
            }
        }

        await pipeWriter.CompleteAsync();
    }
    
    public void Dispose()
    {
        if (_disposed)
            return;
		
        _client.Dispose();

        _disposed = true;
    }
}