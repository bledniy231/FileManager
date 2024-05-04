using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PianoMentor.Certificates
{
	public class PianoMentorServerCertificateManager
	{
		public void Create()
		{
			var rsaKey = RSA.Create(2048);

			string subject = "CN=127.0.0.1";

			var certificateRequest = new CertificateRequest(
				subject,
				rsaKey,
				HashAlgorithmName.SHA256,
				RSASignaturePadding.Pkcs1
			);

			certificateRequest.CertificateExtensions.Add(
				new X509BasicConstraintsExtension(
					certificateAuthority: false,
					hasPathLengthConstraint: false,
					pathLengthConstraint: 0,
					critical: true
				)
			);

			certificateRequest.CertificateExtensions.Add(
				new X509KeyUsageExtension(keyUsages: X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, critical: false)
			);

			certificateRequest.CertificateExtensions.Add(
				new X509SubjectKeyIdentifierExtension(
					key: certificateRequest.PublicKey,
					critical: false
				)
			);

			var sanBuilder = new SubjectAlternativeNameBuilder();
			sanBuilder.AddDnsName("localhost");

			certificateRequest.CertificateExtensions.Add(
				new X509Extension(
					sanBuilder.Build(),
					critical: false
				)
			);


			var expireAt = DateTimeOffset.Now.AddYears(5);

			var certificate = certificateRequest.CreateSelfSigned(DateTimeOffset.Now, expireAt);

			var exportableCertificate = new X509Certificate2(
				certificate.Export(X509ContentType.Cert),
				(string?)null,
				X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet
			).CopyWithPrivateKey(rsaKey);
			exportableCertificate.FriendlyName = "For project of Egor Seryakov's diploma";

			var passwordForCertificateProtection = new SecureString();
			foreach (var @char in "Borismybestfriend111")
			{
				passwordForCertificateProtection.AppendChar(@char);
			}

			File.WriteAllBytes(
				"DiplomaServerCertificationKey.pfx",
				exportableCertificate.Export(
					X509ContentType.Pfx,
					passwordForCertificateProtection
				)
			);

			Console.WriteLine("Сертификат был успешно создан и сохранен в файле DiplomaServerCertificationKey.pfx");
		}

		public void GetCertificateFromStore()
		{
			var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly);

			var certificate = store.Certificates.Where(c => c.FriendlyName.Equals("For project of Egor Seryakov's diploma")).FirstOrDefault();

			if (certificate == null)
			{
				Console.WriteLine("Сертификат не найден");
				return;
			}

			Console.WriteLine($"Сертификат найден: {certificate.FriendlyName}");
			store.Close();
		}
	}
}
