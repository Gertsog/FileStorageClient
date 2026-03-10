namespace FileStorageClient.Models;

public class FileResult : IDisposable, IAsyncDisposable
{
    public Stream Stream { get; }
    public long ContentLength { get; }

    public FileResult(Stream stream, long contentLength)
    {
        Stream = stream;
        ContentLength = contentLength;
    }

    public void Dispose()
    {
        Stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await Stream.DisposeAsync();
    }
}