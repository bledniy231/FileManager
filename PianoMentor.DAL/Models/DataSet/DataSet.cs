using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.DAL.Models.DataSet
{
	public class DataSet
	{
		public long Id { get; set; }
		public long OwnerId { get; set; }
		public DateTime CreatedAt { get; set; }
		/// <summary>
		/// Запись данных на диск была начата, но полностью не завершена.
		/// </summary>
		public bool IsDraft { get; set; }
		public bool IsDeleted { get; set; }

		public PianoMentorUser Owner { get; set; }
		public ICollection<BinaryData> Binaries { get; set; }
		public Storage Storage { get; set; }
	}
}
