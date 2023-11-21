using FileManager.BLL.UploadPercentageChecker;
using FileManager.Contract.Files;
using MediatR;

namespace FileManager.BLL.Files
{
	internal class CheckFilesUploadStatusHandler(
		IPercentageChecker percentageChecker)
		: IRequestHandler<CheckFilesUploadStatusRequest, (int FilesCountAlreadyUploaded, float PercentageCurrentFile)?>
	{
		private readonly IPercentageChecker _percentageChecker = percentageChecker;

		public Task<(int FilesCountAlreadyUploaded, float PercentageCurrentFile)?>
			Handle(CheckFilesUploadStatusRequest request, CancellationToken cancellationToken)
				=> Task.FromResult(_percentageChecker.GetPercentage(request.UserId));
	}
}
