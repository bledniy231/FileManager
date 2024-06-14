using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Files.Download;
using PianoMentor.Contract.Models.DataSet;
using PianoMentor.DAL;

namespace PianoMentor.BLL.UseCases.Files.Download;

public class DownloadUserProfilePhotoHandler(PianoMentorDbContext dbContext) : IRequestHandler<DownloadUserProfilePhotoRequest, DownloadFilesResponse>
{
    public async Task<DownloadFilesResponse> Handle(DownloadUserProfilePhotoRequest request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .Include(u => u.DataSets)
            .ThenInclude(ds => ds.Binaries)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            return new DownloadFilesResponse(null, null, null, [$"Cannot find user with userId {request.UserId}"]);
        }

        try
        {
            var profilePhotoBinary = user.DataSets
                .Where(ds => !ds.IsDraft && !ds.IsDeleted)
                .SelectMany(ds => ds.Binaries)
                .LastOrDefault(b => !b.IsDeleted && b.BinaryTypeId == (int)BinaryTypeEnum.UserProfilePhoto);

            FileInfo? file = null;
            FileStream? fileStream = null;
            string? contentType = null;
            if (profilePhotoBinary != null)
            {
                file = profilePhotoBinary.GetInternalFile();
                if (!file.Exists)
                {
                    return new DownloadFilesResponse(null, null, null,
                    [
                        $"Cannot find a physical file from data set \'{profilePhotoBinary.DataSetId}\' with data id \'{profilePhotoBinary.DataId}\'"
                    ]);
                }

                fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(file.FullName, out contentType))
                {
                    contentType = "application/octet-stream";
                }
            }

            return new DownloadFilesResponse(fileStream, contentType, file?.Extension, null);
        }
        catch (Exception e)
        {
            return new DownloadFilesResponse(null, null, null, [e.Message]);
        }
    }
}