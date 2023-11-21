namespace FileManager.Contract.Files
{
	public class EncryptOneTimeLinkResponse(string oneTimeLink, DateTime linkExpTime)
	{
		public string OneTimeLink { get; set; } = oneTimeLink;
		public DateTime LinkExpirationTime { get; set; } = linkExpTime;
	}
}
