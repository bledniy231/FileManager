using MediatR;
using PianoMentor.BLL.Services.UploadPercentageChecker;
using PianoMentor.Contract.Files;

namespace PianoMentor.BLL.UseCases.Files
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
