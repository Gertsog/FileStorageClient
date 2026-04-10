using Amazon.S3;

namespace FileStorageClient.Abstractions;

public interface IS3ClientFactory
{
    IAmazonS3 CreateClient();
}