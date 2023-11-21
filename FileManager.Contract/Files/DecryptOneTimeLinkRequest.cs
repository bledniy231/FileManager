using MediatR;

namespace FileManager.Contract.Files
{
	public class DecryptOneTimeLinkRequest(string encryptedToken) : IRequest<DecryptOneTimeLinkResponse>
	{
		public string EncryptedToken { get; set; } = encryptedToken;
	}
}
