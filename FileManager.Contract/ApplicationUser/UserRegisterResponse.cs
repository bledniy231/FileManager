﻿namespace FileManager.Contract.ApplicationUser
{
	public class UserRegisterResponse
	{
		public string? Password { get; set; }
		public string? Email { get; set; }

		public string[]? Errors { get; set; } = null;
	}
}
