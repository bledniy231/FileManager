using PianoMentor.Contract.Files;
using PianoMentor.Contract.Models.DataSet;
using PianoMentor.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace PianoMentor.BLL.Files
{
	internal class GetListOfUploadedFilesHandler(
		PianoMentorDbContext dbContext) 
		: IRequestHandler<GetListOfUploadedFilesRequest, GetListOfUploadedFilesResponse>
	{
		public async Task<GetListOfUploadedFilesResponse> Handle(GetListOfUploadedFilesRequest request, CancellationToken cancellationToken)
		{
			var dataSets = await dbContext.DataSets
				.AsNoTracking()
				.Include(ds => ds.Binaries)
				.Where(ds => ds.OwnerId == request.UserId)
				.ToListAsync();

			if (dataSets == null)
			{
				return new GetListOfUploadedFilesResponse(null, [$"Cannot find data sets for user with userId: {request.UserId}"]);
			}

			var dataSetModels = dataSets.Select(ds => new DataSetModel
			{
				OwnerId = ds.OwnerId,
				Id = ds.Id,
				CreatedAt = ds.CreatedAt,
				Binaries = ds.Binaries.Select(b => new BinaryDataModel
				{
					DataId = b.DataId,
					Filename = b.Filename,
					Length = b.Length
				}).ToList()
			}).ToList();

			return new GetListOfUploadedFilesResponse(dataSetModels, null);
		}
	}
}
