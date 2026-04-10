namespace FileStorageClient.Models
{
	public class FileStorageCredentials
	{
		public string UserName { get; }
		public string Password { get; }

		public FileStorageCredentials(string userName, string password)
		{
			UserName = userName;
			Password = password;
		}
	}
}