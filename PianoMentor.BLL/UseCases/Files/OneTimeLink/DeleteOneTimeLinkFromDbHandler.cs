using System.Web;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Files.OneTimeLink;
using PianoMentor.DAL;

namespace PianoMentor.BLL.UseCases.Files.OneTimeLink
{
	internal class DeleteOneTimeLinkFromDbHandler(
		PianoMentorDbContext dbContext) 
		: IRequestHandler<DeleteOneTimeLinkFromDbRequest, Unit>
	{
		public async Task<Unit> Handle(DeleteOneTimeLinkFromDbRequest request, CancellationToken cancellationToken)
		{
			string encToken = HttpUtility.UrlEncode(request.UrlEncryptedToken.ToString());

			var oneTimeLink = await dbContext.OneTimeLinks
				.FirstAsync(ol => ol.UrlEncryptedToken.Equals(encToken), cancellationToken);

			dbContext.OneTimeLinks.Remove(oneTimeLink);
			await dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
