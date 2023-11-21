namespace FileManager.Contract.Models
{
	public class JwtTokensModel
	{
		public string? AccessToken { get; set; }
		public string? RefreshToken { get; set; }
	}
}
