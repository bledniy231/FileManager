using Microsoft.AspNetCore.Identity;

namespace FileManager.DAL.Domain.Identity
{
	public class FileManagerUser : IdentityUser<long>
	{
		//public DateTime AccessTokenExpireTime { get; set; } = DateTime.MinValue;
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpireTime { get; set; }
		public ICollection<DataSet.DataSet> DataSets { get; set; }
	}
}
