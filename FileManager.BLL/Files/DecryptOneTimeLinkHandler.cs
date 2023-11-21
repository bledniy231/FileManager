using FileManager.BLL.CryptoLinkManager;
using FileManager.Contract.Files;
using MediatR;
using System.Globalization;
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
			string encryptedFromHttpToken = HttpUtility.UrlDecode(request.EncryptedToken);
			string decryptedToken = await _cryptoLinkManager.DecryptAsync(encryptedFromHttpToken);

			string[] tokenParts = decryptedToken.Split('_');

			return tokenParts.Length switch
			{
				>= 2 and <= 3 when IsValidTime(tokenParts[0]) => new DecryptOneTimeLinkResponse
				{
					DataSetId = long.Parse(tokenParts[1]),
					DataId = int.TryParse(tokenParts[2], out int dataId) ? dataId : null
				},
				_ => throw new ArgumentException("Wrong decrypted token")
			};
		}

		private static bool IsValidTime(string expTimeString)
			=> DateTime.ParseExact(expTimeString, "yyyyMMddHHmm", CultureInfo.InvariantCulture) > DateTime.UtcNow
			? true 
			: throw new Exception("Token had been expired");
	}
}
