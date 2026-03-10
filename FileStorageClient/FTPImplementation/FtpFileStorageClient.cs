using System.Net;
using FileStorageClient.Abstractions;
using FileStorageClient.Exceptions;
using FileStorageClient.Models;
using FluentFTP;
using FileInfo = FileStorageClient.Models.FileInfo;

namespace FileStorageClient.FTPImplementation
{
	internal class FtpFileStorageClient : IFileStorageClient
	{
		private readonly string _ftpAddress;
		private readonly string _ftpUsername;
		private readonly string _ftpPassword;

		public FtpFileStorageClient(
			string ftpAddress,
			string ftpUsername,
			string ftpPassword)
		{
			if (string.IsNullOrEmpty(ftpAddress))
				throw new ArgumentNullException(nameof(ftpAddress));
			if (string.IsNullOrEmpty(ftpUsername))
				throw new ArgumentNullException(nameof(ftpUsername));
			if (string.IsNullOrEmpty(ftpPassword))
				throw new ArgumentNullException(nameof(ftpPassword));
			
			_ftpAddress = ftpAddress;
			_ftpUsername = ftpUsername;
			_ftpPassword = ftpPassword;
		}
		
		public async Task UploadFileAsync(
			FileInfo fileInfo,
			Stream fileStream,
			CancellationToken ct = default)
		{
			using var ftp = await GetFtpClientAsync(_ftpAddress, _ftpUsername, _ftpPassword);

			try
			{
				await ftp.UploadAsync(fileStream, fileInfo.Path, FtpRemoteExists.Overwrite, true, token: ct);
			}
			catch (WebException e)
			{
				throw new UploadException($"Attempt to upload file \"{fileInfo.Path}\" to FTP is failed.", e);
			}
		}

		public async Task<FileResult> DownloadFileAsync(
			FileInfo fileInfo,
			CancellationToken ct = default)
		{
			var downloadStream = new MemoryStream();

			try
			{
				using var ftp = await GetFtpClientAsync(_ftpAddress, _ftpUsername, _ftpPassword);

				if (!await ftp.DownloadAsync(downloadStream, fileInfo.Path, token: ct))
					throw new DownloadException($"Unable to download file: {fileInfo.Path}");
		
				if (downloadStream.CanSeek)
					downloadStream.Seek(0, SeekOrigin.Begin);
			}
			catch (WebException e)
			{
				throw new DownloadException($"Attempt to download file \"{fileInfo.Path}\" from FTP is failed.", e);
			}

			return new FileResult(downloadStream, downloadStream.Length);
		}

		private static async Task<FtpClient> GetFtpClientAsync(string host, string username, string password)
		{
			var ftp = new FtpClient(host, username, password);
			await ftp.ConnectAsync(new FtpProfile
			{
				RetryAttempts = 1,
				Host = host,
				Credentials = new NetworkCredential
				{
					UserName = username,
					Password = password
				},
				Encoding = System.Text.Encoding.UTF8,
				Timeout = 120 * 1000
			});
			
			return ftp;
		}
	}
} 