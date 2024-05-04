using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.DataSet;

namespace PianoMentor.Contract.Files
{
    public class GetListOfUploadedFilesResponse(
		ICollection<DataSetModel> dataSetModels,
		string[]? errors = null) : DefaultResponse(errors)
	{
		public ICollection<DataSetModel> DataSetsModels { get; set; } = dataSetModels;
	}
}
