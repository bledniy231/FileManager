using System.Globalization;
using MediatR;
using PianoMentor.BLL.Services.CryptoLinkManager;
using PianoMentor.Contract.Files.OneTimeLink;

namespace PianoMentor.BLL.UseCases.Files.OneTimeLink
{
	internal class DecryptOneTimeLinkHandler(
		ICryptoLinkManager downloadLinkManager)
		: IRequestHandler<DecryptOneTimeLinkRequest, DecryptOneTimeLinkResponse>
	{
		public async Task<DecryptOneTimeLinkResponse> Handle(DecryptOneTimeLinkRequest request, CancellationToken cancellationToken)
		{
			string encryptedFromHttpToken;
			string decryptedToken;
			try
			{
				encryptedFromHttpToken = Uri.UnescapeDataString(request.EncryptedToken);
				decryptedToken = await downloadLinkManager.DecryptAsync(encryptedFromHttpToken);
			}
			catch (Exception ex)
			{
				return new DecryptOneTimeLinkResponse(-1, null, [ex.Message]);
			}

			string[] tokenParts = decryptedToken.Split('_');

			return tokenParts.Length switch
			{
				>= 2 and <= 3 when 
				IsValidTime(tokenParts[0]) &&
				long.TryParse(tokenParts[1], out long dataSetIdParsed) 
				=> new DecryptOneTimeLinkResponse
				(
					dataSetIdParsed,
					int.TryParse(tokenParts[2], out int dataId) ? dataId : null,
					null
				),
				_ => new DecryptOneTimeLinkResponse(-1, null, ["Wrong one time link token"])
			};
		}

		private static bool IsValidTime(string expTimeString)
			=> DateTime.ParseExact(expTimeString, "yyyyMMddHHmm", CultureInfo.InvariantCulture) > DateTime.UtcNow;
	}
}
