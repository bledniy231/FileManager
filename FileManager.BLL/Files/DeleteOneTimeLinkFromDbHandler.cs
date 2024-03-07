using FileManager.Contract.Files;
using FileManager.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace FileManager.BLL.Files
{
	internal class DeleteOneTimeLinkFromDbHandler(
		FileManagerDbContext dbContext) 
		: IRequestHandler<DeleteOneTimeLinkFromDbRequest, Unit>
	{
		private readonly FileManagerDbContext _dbContext = dbContext;

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
