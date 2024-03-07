using FileManager.BLL.CryptoLinkManager;
using FileManager.Contract.Files;
using MediatR;
using System.Globalization;
using System.Net;
using System.Web;

namespace FileManager.BLL.Files
{
	internal class DecryptOneTimeLinkHandler(
		ICryptoLinkManager downloadLinkManager)
		: IRequestHandler<DecryptOneTimeLinkRequest, DecryptOneTimeLinkResponse>
	{
		private readonly ICryptoLinkManager _cryptoLinkManager = downloadLinkManager;

		public async Task<DecryptOneTimeLinkResponse> Handle(DecryptOneTimeLinkRequest request, CancellationToken cancellationToken)
		{
			string encryptedFromHttpToken;
			string decryptedToken;
			try
			{
				encryptedFromHttpToken = Uri.UnescapeDataString(request.EncryptedToken);
				decryptedToken = await _cryptoLinkManager.DecryptAsync(encryptedFromHttpToken);
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
