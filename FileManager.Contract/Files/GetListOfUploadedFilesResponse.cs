using FileManager.Contract.Default;
using FileManager.Contract.Models.DataSet;

namespace FileManager.Contract.Files
{
    public class GetListOfUploadedFilesResponse(
		ICollection<DataSetModel> dataSetModels,
		string[]? errors = null) : DefaultResponse(errors)
	{
		public ICollection<DataSetModel> DataSetsModels { get; set; } = dataSetModels;
	}
}
