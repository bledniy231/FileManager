using MediatR;

namespace PianoMentor.Contract.Files.OneTimeLink
{
	public class DecryptOneTimeLinkRequest(string encryptedToken) : IRequest<DecryptOneTimeLinkResponse>
	{
		public string EncryptedToken { get; set; } = encryptedToken;
	}
}
