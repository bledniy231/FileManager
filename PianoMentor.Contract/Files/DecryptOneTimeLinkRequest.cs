using MediatR;

namespace PianoMentor.Contract.Files
{
	public class DecryptOneTimeLinkRequest(string encryptedToken) : IRequest<DecryptOneTimeLinkResponse>
	{
		public string EncryptedToken { get; set; } = encryptedToken;
	}
}
