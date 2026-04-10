using Amazon.S3;
using FileStorageClient.Abstractions;
using Microsoft.Extensions.Options;

namespace FileStorageClient.S3Implementation;

internal class S3ClientFactory : IS3ClientFactory
{
    private readonly FileStorageOptions _options;

    public S3ClientFactory(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

    public IAmazonS3 CreateClient()
    {
        var config = new AmazonS3Config
        {
            ServiceURL = _options.FileStorageUrl
        };
        return new AmazonS3Client(_options.FileStorageUserName, _options.FileStoragePassword, config);
    }
}