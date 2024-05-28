using PianoMentor.BLL.UploadPercentageChecker;
using PianoMentor.Contract.Files;
using MediatR;

namespace PianoMentor.BLL.Files
{
	internal class CheckFilesUploadStatusHandler(
		IPercentageChecker percentageChecker)
		: IRequestHandler<CheckFilesUploadStatusRequest, (int FilesCountAlreadyUploaded, float PercentageCurrentFile)?>
	{
		public Task<(int FilesCountAlreadyUploaded, float PercentageCurrentFile)?>
			Handle(CheckFilesUploadStatusRequest request, CancellationToken cancellationToken)
				=> Task.FromResult(percentageChecker.GetPercentage(request.UserId));
	}
}
