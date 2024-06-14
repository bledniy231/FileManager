﻿namespace PianoMentor.DAL.Models.DataSet
{
	public class OneTimeLink
	{
		public DateTime LinkExpirationTime { get; set; }
		public string UrlEncryptedToken { get; set; }
		public bool IsUsed { get; set; } = false;
	}
}
