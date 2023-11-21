using MediatR;

namespace FileManager.Contract.Files
{
	public class DeleteOneTimeLinkFromDbRequest(string urlEncryptedToken) : IRequest<Unit>
	{
		public string UrlEncryptedToken { get; set; } = urlEncryptedToken;
	}
}
