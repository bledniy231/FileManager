using MediatR;
using PianoMentor.BLL.Services.FilesUploadManagers;
using PianoMentor.Contract.Files.Upload;
using PianoMentor.Contract.Models.DataSet;
using PianoMentor.DAL;

namespace PianoMentor.BLL.UseCases.Files.Upload
{
	internal class UploadQuestionImageHandler(
		PianoMentorDbContext dbContext,
		IUploadFilesManager uploadFilesManager) : IRequestHandler<UploadQuestionImageRequest, UploadFilesResponse>
	{
		public async Task<UploadFilesResponse> Handle(UploadQuestionImageRequest request, CancellationToken cancellationToken)
		{
			var questionDb = dbContext.QuizQuestions.FirstOrDefault(q => q.QuestionId == request.QuestionId && !q.IsDeleted);
			if (questionDb == null)
			{
				return new UploadFilesResponse(0, [$"No question with {request.QuestionId} found in DB"]);
			}

			var uploadingResult = await uploadFilesManager.UploadAsync(request.UserId, BinaryTypeEnum.QuizQuestionFile, request.ContentType, request.Body, cancellationToken);
			if (uploadingResult is { Errors: not null, NewDataSet: null } or { Errors: null, NewDataSet: null })
			{
				return new UploadFilesResponse(0, uploadingResult.Errors);
			}

			questionDb.AttachedDataSetId = uploadingResult.NewDataSet.Id; 
			dbContext.SaveChanges();

			return new UploadFilesResponse(0, null);
		}
	}
}
