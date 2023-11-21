using MediatR;

namespace FileManager.Contract.Files
{
	public class GetListOfUploadedFilesRequest(long userId) : IRequest<GetListOfUploadedFilesResponse>
	{
		public long UserId { get; set; } = userId;
	}
}
