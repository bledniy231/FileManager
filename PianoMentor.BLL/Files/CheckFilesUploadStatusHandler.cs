using PianoMentor.BLL.UploadPercentageChecker;
using PianoMentor.Contract.Files;
using MediatR;

namespace PianoMentor.BLL.Files
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
