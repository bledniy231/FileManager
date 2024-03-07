namespace FileManager.Contract.Default
{
	public class DefaultResponse(string[]? errors)
	{
		/// <summary>
		/// Свойство, отвечающее за хранение ошибок в случае неудачного выполнения запроса
		/// Будет null, если запрос выполнен успешно
		/// </summary>
		public string[]? Errors { get; set; } = errors;
	}
}
