namespace FileStorageClient.Exceptions
{
	internal class UploadException : Exception
	{
		public UploadException() { }

		public UploadException(string msg) : base(msg) { }

		public UploadException(string msg, Exception inner) : base(msg, inner) { }
	}
}
