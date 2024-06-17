using MediatR;
using PianoMentor.BLL.Services.FilesUploadManagers;
using PianoMentor.Contract.Files.Upload;
using PianoMentor.Contract.Models.DataSet;
using PianoMentor.DAL;

namespace PianoMentor.BLL.UseCases.Files.Upload;

internal class UploadCourseItemFileHandler(
    PianoMentorDbContext dbContext,
    IUploadFilesManager uploadFilesManager) : IRequestHandler<UploadCourseItemFileRequest, UploadFilesResponse>
{
    public async Task<UploadFilesResponse> Handle(UploadCourseItemFileRequest request, CancellationToken cancellationToken)
    {
        var courseItem = dbContext.CourseItems.FirstOrDefault(ci => ci.CourseItemId == request.CourseItemId && !ci.IsDeleted);
        if (courseItem == null)
        {
            return new UploadFilesResponse(0, ["Incorrect course item Id or course item was deleted"]);
        }
        
        uploadFilesManager.AllowOnlyOneFileUpload();
        uploadFilesManager.AddCheckingFileExtensions(FileExtensionsCollectionsEnum.Documents);
        var uploadingResult = await uploadFilesManager.UploadAsync(request.UserId, BinaryTypeEnum.CourseItemFile, request.ContentType, request.Body, cancellationToken);
        if (uploadingResult is { Errors: not null, NewDataSet: null } or { Errors: null, NewDataSet: null })
        {
            return new UploadFilesResponse(0, uploadingResult.Errors);
        }
        
        courseItem.AttachedDataSetId = uploadingResult.NewDataSet.Id;
        dbContext.SaveChanges();
        
        return new UploadFilesResponse(uploadingResult.NewDataSet.Id);
    }
}