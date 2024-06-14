using MediatR;

namespace PianoMentor.Contract.Files.OneTimeLink
{
	public class EncryptOneTimeLinkRequest(long dataSetId, int? dataId) : IRequest<EncryptOneTimeLinkResponse>
	{
		public long DataSetId { get; set; } = dataSetId;
		/// <summary>
		/// Будет null тогда, когда пользователь хочет зашифровать ссылку на весь набор данных
		/// </summary>
		public int? DataId { get; set; } = dataId;
	}
}
