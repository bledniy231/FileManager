using Microsoft.AspNetCore.Identity;

namespace PianoMentor.DAL.Domain.Identity
{
	public class PianoMentorUser : IdentityUser<long>
	{
		//public DateTime AccessTokenExpireTime { get; set; } = DateTime.MinValue;
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpireTime { get; set; }
		public bool IsDeleted { get; set; }
		public ICollection<DataSet.DataSet> DataSets { get; set; }
	}
}
