using MediatR;

namespace PianoMentor.Contract.Files
{
	public class GetListOfUploadedFilesRequest(long userId) : IRequest<GetListOfUploadedFilesResponse>
	{
		public long UserId { get; set; } = userId;
	}
}
