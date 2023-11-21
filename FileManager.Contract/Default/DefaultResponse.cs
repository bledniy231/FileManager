namespace FileManager.Contract.Default
{
	public class DefaultResponse(bool isSuccess, string[]? errors)
	{
		public bool IsSuccess { get; set; } = isSuccess;
		public string[]? Errors { get; set; } = errors;
	}
}
