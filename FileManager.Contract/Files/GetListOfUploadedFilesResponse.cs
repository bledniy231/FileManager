using FileManager.Contract.Models;

namespace FileManager.Contract.Files
{
	public class GetListOfUploadedFilesResponse
	{
		public ICollection<DataSetModel> DataSetsModels { get; set; }
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
	}
}
