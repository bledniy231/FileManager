﻿using FileManager.Contract.Files;
using FileManager.Contract.Models;
using FileManager.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FileManager.BLL.Files
{
	internal class GetListOfUploadedFilesHandler(
		FileManagerDbContext dbContext) 
		: IRequestHandler<GetListOfUploadedFilesRequest, GetListOfUploadedFilesResponse>
	{
		private readonly FileManagerDbContext _dbContext = dbContext;

		public async Task<GetListOfUploadedFilesResponse> Handle(GetListOfUploadedFilesRequest request, CancellationToken cancellationToken)
		{
			var dataSets = await _dbContext.DataSets
				.AsNoTracking()
				.Include(ds => ds.Binaries)
				.Where(ds => ds.OwnerId == request.UserId)
				.ToListAsync();

			if (dataSets == null)
			{
				return new GetListOfUploadedFilesResponse
				{
					IsSuccess = false,
					Message = $"Cannot find data sets for user with userId: {request.UserId}"
				};
			}

			return new GetListOfUploadedFilesResponse
			{
				DataSetsModels = dataSets.Select(ds => new DataSetModel
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
				}).ToList(),
				IsSuccess = true
			};
		}
	}
}
