namespace FileStorageClient.Models
{
	public class FileInfo
	{
		public string Path { get; set; }
		public string ContentType { get; set; }
		
		public FileInfo(string path, string contentType)
		{
			Path = path;
			ContentType = contentType;
		}
		
		public FileInfo(string path) : this(path, string.Empty)
		{
		}
	}
}