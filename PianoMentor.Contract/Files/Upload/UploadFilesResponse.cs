using PianoMentor.Contract.Default;

namespace PianoMentor.Contract.Files.Upload;

public class UploadFilesResponse(long dataSetId, string[]? errors = null) : DefaultResponse(errors)
{
    public long DataSetId { get; set; } = dataSetId;
}