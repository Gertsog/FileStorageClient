using Amazon.S3;
using Amazon.S3.Model;
using FileStorageClient.Abstractions;
using FileStorageClient.Models;
using FileInfo = FileStorageClient.Models.FileInfo;

namespace FileStorageClient.S3Implementation
{
	internal class S3FileStorageClient : IDisposable, IFileStorageClient
	{
		private readonly IAmazonS3 _client;
		private readonly string _bucketName;
		
		private bool _disposed;

		public S3FileStorageClient(IAmazonS3 client, string bucketName)
		{
			_client = client;
			_bucketName = bucketName;
		}
		
		public async Task UploadFileAsync(FileInfo fileInfo, Stream fileStream, CancellationToken ct = default)
		{
			var request = new PutObjectRequest
			{
				BucketName = _bucketName,
				Key = fileInfo.Path,
				InputStream = fileStream,
				UseChunkEncoding = false,
				AutoCloseStream = false
			};
			
			if (!string.IsNullOrEmpty(fileInfo.ContentType))
				request.ContentType = fileInfo.ContentType;

			await _client.PutObjectAsync(request, ct);
		}
		
		public async Task<FileResult> DownloadFileAsync(FileInfo fileInfo, CancellationToken ct = default)
		{
			var filePath = fileInfo.Path.TrimStart('/');
			var objectRequest = new GetObjectRequest { BucketName = _bucketName, Key = filePath };
			var objectResponse = await _client.GetObjectAsync(objectRequest, ct);
			
			return new FileResult(objectResponse.ResponseStream, objectResponse.ContentLength);
		}
		
		public void Dispose()
		{
			if (_disposed)
				return;
		
			_client.Dispose();

			_disposed = true;
		}
	}
}