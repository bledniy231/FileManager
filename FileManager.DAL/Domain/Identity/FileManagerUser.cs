using Microsoft.AspNetCore.Identity;

namespace FileManager.DAL.Domain.Identity
{
	public class FileManagerUser : IdentityUser<long>
	{
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpireTime { get; set; }
		public ICollection<DataSet.DataSet> DataSets { get; set; }
	}
}
