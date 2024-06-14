using MediatR;
using PianoMentor.BLL.Services.FilesUploadManagers;
using PianoMentor.Contract.Files.Upload;
using PianoMentor.Contract.Models.DataSet;
using PianoMentor.DAL;

namespace PianoMentor.BLL.UseCases.Files.Upload
{
	internal class UploadUsersFilesHandler(
		PianoMentorDbContext dbContext,
		IUploadFilesManager uploadFilesManager) 
		: IRequestHandler<UploadUsersFilesRequest, UploadFilesResponse>
	{
		public async Task<UploadFilesResponse> Handle(UploadUsersFilesRequest request, CancellationToken cancellationToken)
		{
			var uploadingResult = await uploadFilesManager.UploadAsync(request.UserId, BinaryTypeEnum.UserFile, request.ContentType, request.Body, cancellationToken);
			if (uploadingResult is { Errors: not null, NewDataSet: null } or { Errors: null, NewDataSet: null })
			{
				return new UploadFilesResponse(0, uploadingResult.Errors);
			}
        
			return new UploadFilesResponse(uploadingResult.NewDataSet.Id);
		}
	}
}
