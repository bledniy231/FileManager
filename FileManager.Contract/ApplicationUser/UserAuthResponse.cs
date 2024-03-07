namespace FileManager.Contract.ApplicationUser
{
	public class UserAuthResponse
	{
		public string? AccessToken { get; set; }
		public string? RefreshToken { get; set; }
		public string? Email { get; set; }
		public string? UserName { get; set; }
		public IEnumerable<string>? Roles { get; set; }
		public long Id { get; set; }


		public bool IsSuccess { get; set; }
		public string? FailedMessage { get; set; }
	}
}
