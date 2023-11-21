using FileManager.BLL.CryptoLinkManager;
using FileManager.Contract.Files;
using FileManager.DAL;
using MediatR;
using System.Web;

namespace FileManager.BLL.Files
{
	internal class EncryptOneTimeLinkHandler(
		FileManagerDbContext dbContext,
		ICryptoLinkManager downloadLinkManager)
		: IRequestHandler<EncryptOneTimeLinkRequest, EncryptOneTimeLinkResponse>
	{
		private readonly FileManagerDbContext _dbContext = dbContext;
		private readonly ICryptoLinkManager _cryptoLinkManager = downloadLinkManager;

		public async Task<EncryptOneTimeLinkResponse> Handle(EncryptOneTimeLinkRequest request, CancellationToken cancellationToken)
		{
			var expTime = DateTime.UtcNow.AddDays(1);

			string plainTextToEncrypt = $"{expTime:yyyyMMddHHmm}_{request.DataSetId}_{request.DataId}";
			string encryptedToken = await _cryptoLinkManager.EncryptAsync(plainTextToEncrypt);

			var urlEncryptedToken = HttpUtility.UrlEncode(encryptedToken);

			_dbContext.OneTimeLinks.Add(new DAL.Domain.DataSet.OneTimeLink
			{
				LinkExpirationTime = expTime,
				UrlEncryptedToken = urlEncryptedToken
			});
			await _dbContext.SaveChangesAsync(cancellationToken);

			return new EncryptOneTimeLinkResponse($"api/Files/DownloadFilesViaLink?token={urlEncryptedToken}", expTime);
		}
	}
}
