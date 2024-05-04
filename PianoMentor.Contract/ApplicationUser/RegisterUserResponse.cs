namespace PianoMentor.Contract.ApplicationUser
{
	public class RegisterUserResponse
	{
		public string? Password { get; set; }
		public string? Email { get; set; }

		public string[]? Errors { get; set; } = null;
	}
}
