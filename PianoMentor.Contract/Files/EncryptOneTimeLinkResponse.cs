using PianoMentor.Contract.Default;

namespace PianoMentor.Contract.Files
{
	public class EncryptOneTimeLinkResponse(
		string? oneTimeLink, 
		DateTime? linkExpTime,
		string[]? errors) : DefaultResponse(errors)
	{
		/// <summary>
		/// Ссылка для скачивания файла
		/// Будет null, когда произошла ошибка в процессе создания ссылки или сохранения ссылки в БД
		/// </summary>
		public string? OneTimeLink { get; set; } = oneTimeLink;
		/// <summary>
		/// Время истечения ссылки
		/// Будет null, когда произошла ошибка в процессе создания ссылки или сохранения ссылки в БД
		/// </summary>
		public DateTime? LinkExpirationTime { get; set; } = linkExpTime;
	}
}
