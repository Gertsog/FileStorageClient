using System.ComponentModel.DataAnnotations;

namespace FileStorageClient
{
	/// <summary>
	/// Настройки подключения к файловому хранилищу.
	/// </summary>
	internal class FileStorageOptions
	{
		/// <summary>
		/// Адрес файлового хранилища.
		/// </summary>
		[Required(ErrorMessage = "Не задана настройка подключения к: Адрес файлового хранилища")]
		public string FileStorageUrl { get; set; } = null!;
		
		/// <summary>
		/// Имя пользователя.
		/// </summary>
		[Required(ErrorMessage = "Не задана настройка подключения к: Имя пользователя файлового хранилища")]
		public string FileStorageUserName { get; set; } = null!;

		/// <summary>
		/// Имя пользователя.
		/// </summary>
		[Required(ErrorMessage = "Не задана настройка подключения к: Пароль пользователя файлового хранилища")]
		public string FileStoragePassword { get; set; } = null!;

		/// <summary>
		/// Наименование бакета (если используется S3).
		/// </summary>
		public string BucketName { get; set; } = null!;
		
		/// <summary>
		/// Тип используемого файлового хранилища (S3, Ftp).
		/// </summary>
		public FileStorageType FileStorageType { get; set; }
	}
}