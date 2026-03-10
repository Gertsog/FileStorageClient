namespace FileStorageClient.Exceptions
{
	internal class DownloadException : Exception
	{
		public DownloadException() { }

		public DownloadException(string msg) : base(msg) { }

		public DownloadException(string msg, Exception inner) : base(msg, inner) { }
	}
}
