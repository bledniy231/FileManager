using MediatR;

namespace PianoMentor.Contract.Files.OneTimeLink
{
	public class DeleteOneTimeLinkFromDbRequest(string urlEncryptedToken) : IRequest<Unit>
	{
		public string UrlEncryptedToken { get; set; } = urlEncryptedToken;
	}
}
