using FileStorageClient.Abstractions;
using FileStorageClient.FTPImplementation;
using FileStorageClient.S3Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileStorageClient.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions<FileStorageOptions>()
				.Bind(configuration)
				.ValidateDataAnnotations();

			services.AddTransient<IS3ClientFactory, S3ClientFactory>();
			services.AddScoped<IFileStorageClient>(sp =>
			{
				var options = sp.GetRequiredService<IOptions<FileStorageOptions>>().Value;
				var factory = sp.GetRequiredService<IS3ClientFactory>();

				return options.FileStorageType switch
				{
					FileStorageType.Ftp => new FtpFileStorageClient(
						options.FileStorageUrl,
						options.FileStorageUserName,
						options.FileStoragePassword),
					
					FileStorageType.S3 => new S3FileStorageClient(
							factory.CreateClient(),
							options.BucketName),
					
					_ => throw new ArgumentOutOfRangeException(nameof(options.FileStorageType),
						$"Unknown file storage type {options.FileStorageType}.")
				};
			});
			
			return services;
		}
	}
}