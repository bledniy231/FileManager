using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace PianoMentor.BLL.CryptoLinkManager
{
	public class CryptoLinkManagerViaAes(
		IConfiguration config)
		: ICryptoLinkManager
	{
		private readonly string _encryptionKey = config.GetValue<string>("OneTimeLinksEncryptionKey")
			?? throw new ArgumentNullException("Cannot find encription key for one time links");

		public async Task<string> EncryptAsync(string plainText)
		{
			if (plainText == null || plainText.Length <= 0)
			{
				throw new ArgumentNullException(nameof(plainText));
			}

			using var aesAlg = Aes.Create();
			aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionKey);
			aesAlg.Mode = CipherMode.CBC; // "Cipher Block Chaining" (Цепочка блоков шифрования)
			aesAlg.Padding = PaddingMode.PKCS7; // Метод дополнения блоков данных, если их размер не является кратным размеру блока шифрования

			using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

			using var memoryStream = new MemoryStream();
			using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
			{
				using var streamWriter = new StreamWriter(cryptoStream);
				await streamWriter.WriteAsync(plainText);
			}

			byte[] iv = aesAlg.IV;
			byte[] encryptedBytes = memoryStream.ToArray();

			// Объединение вектора инициализации (IV) и зашифрованных данных.
			// Нужно для того, чтобы каждый раз шифр был уникальным
			byte[] result = new byte[iv.Length + encryptedBytes.Length];
			Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
			Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

			return Convert.ToBase64String(result);
		}

		public async Task<string> DecryptAsync(string encryptedToken)
		{
			if (encryptedToken == null || encryptedToken.Length <= 0)
			{
				throw new ArgumentNullException(nameof(encryptedToken));
			}

			using var aesAlg = Aes.Create();
			aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionKey);
			aesAlg.Mode = CipherMode.CBC;
			aesAlg.Padding = PaddingMode.PKCS7;

			// Извлечение вектора инициализации (IV) из начала зашифрованных данных
			byte[] encryptedBytesWithIV = Convert.FromBase64String(encryptedToken);
			byte[] iv = new byte[aesAlg.BlockSize / 8];
			Buffer.BlockCopy(encryptedBytesWithIV, 0, iv, 0, iv.Length);

			using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv);

			using var memoryStream = new MemoryStream(encryptedBytesWithIV, iv.Length, encryptedBytesWithIV.Length - iv.Length);
			using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			using var streamReader = new StreamReader(cryptoStream);
			return await streamReader.ReadToEndAsync();
		}
	}
}
