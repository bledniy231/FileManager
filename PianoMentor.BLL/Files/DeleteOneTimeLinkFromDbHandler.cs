using PianoMentor.Contract.Files;
using PianoMentor.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace PianoMentor.BLL.Files
{
	internal class DeleteOneTimeLinkFromDbHandler(
		PianoMentorDbContext dbContext) 
		: IRequestHandler<DeleteOneTimeLinkFromDbRequest, Unit>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;

		public async Task<Unit> Handle(DeleteOneTimeLinkFromDbRequest request, CancellationToken cancellationToken)
		{
			string encToken = HttpUtility.UrlEncode(request.UrlEncryptedToken.ToString());

			var oneTimeLink = await _dbContext.OneTimeLinks
				.FirstAsync(ol => ol.UrlEncryptedToken.Equals(encToken), cancellationToken);

			_dbContext.OneTimeLinks.Remove(oneTimeLink);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
