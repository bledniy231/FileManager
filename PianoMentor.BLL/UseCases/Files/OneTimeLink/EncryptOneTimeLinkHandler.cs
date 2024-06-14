using MediatR;
using PianoMentor.BLL.Services.CryptoLinkManager;
using PianoMentor.Contract.Files.OneTimeLink;
using PianoMentor.DAL;

namespace PianoMentor.BLL.UseCases.Files.OneTimeLink
{
	internal class EncryptOneTimeLinkHandler(
		PianoMentorDbContext dbContext,
		ICryptoLinkManager downloadLinkManager)
		: IRequestHandler<EncryptOneTimeLinkRequest, EncryptOneTimeLinkResponse>
	{
		public async Task<EncryptOneTimeLinkResponse> Handle(EncryptOneTimeLinkRequest request, CancellationToken cancellationToken)
		{
			var expTime = DateTime.UtcNow.AddDays(1);

			string plainTextToEncrypt = $"{expTime:yyyyMMddHHmm}_{request.DataSetId}_{request.DataId}";
			string encryptedToken;
			string? urlEncryptedToken;
			try
			{
				encryptedToken = await downloadLinkManager.EncryptAsync(plainTextToEncrypt);
				urlEncryptedToken = Uri.EscapeDataString(encryptedToken);
			}
			catch (Exception ex)
			{
				return new EncryptOneTimeLinkResponse(null, expTime, [ex.Message]);
			}

			dbContext.OneTimeLinks.Add(new DAL.Models.DataSet.OneTimeLink
			{
				LinkExpirationTime = expTime,
				UrlEncryptedToken = urlEncryptedToken
			});

			try
			{
				await dbContext.SaveChangesAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				return new EncryptOneTimeLinkResponse(null, expTime, [ex.Message]);
			}

			return new EncryptOneTimeLinkResponse($"api/Files/DownloadFilesViaLink?token={urlEncryptedToken}", expTime, null);
		}
	}
}
