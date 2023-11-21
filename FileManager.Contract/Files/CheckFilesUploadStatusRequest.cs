using MediatR;

namespace FileManager.Contract.Files
{
	public class CheckFilesUploadStatusRequest(long userId) 
		: IRequest<(int FilesCountAlreadyUploaded, float PercentageCurrentFile)?>
	{
		public long UserId { get; set; } = userId;
	}
}
