namespace PianoMentor.BLL.Services.CryptoLinkManager
{
	public interface ICryptoLinkManager
	{
		public Task<string> EncryptAsync(string plainText);
		public Task<string> DecryptAsync(string encryptedToken);
	}
}
