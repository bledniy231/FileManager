using FileManager.Contract.Courses;
using FileManager.Contract.Models.PianoMentor.Courses;
using FileManager.DAL;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.BLL.Couses
{
	internal class GetCourseItemsHandler(FileManagerDbContext dbContext) : IRequestHandler<GetCourseItemsRequest, GetCourseItemsResponse>
	{
		private readonly FileManagerDbContext _dbContext = dbContext;

		public Task<GetCourseItemsResponse> Handle(GetCourseItemsRequest request, CancellationToken cancellationToken)
		{
			var courseItemsUserProgressesNames = _dbContext.CourseItemUserProgresses
				.Where(ciup => 
					ciup.UserId == request.UserId
					&& ciup.CourseItem.CourseId == request.CourseId)
				.Select(ciup => new
				{
					ciup.CourseItemId,
					ProgressName = ciup.CourseItemProgressType.Name
				})
				.ToList();

			var courseItems = _dbContext.CourseItems
				.Where(ci => ci.CourseId == request.CourseId)
				.Select(ci => new CourseItemModel
				{
					CourseItemId = ci.CourseItemId,
					Position = ci.Position,
					Title = ci.Title,
					CourseId = ci.CourseId,
					CourseItemType = ci.CourseItemType.Name,
					CourseItemProgressType = courseItemsUserProgressesNames
						.Where(ciupn => ciupn.CourseItemId == ci.CourseItemId)
						.Select(ciupn => ciupn.ProgressName)
						.DefaultIfEmpty("NotStarted")
						.FirstOrDefault()!
				})
				.ToList();

			return Task.FromResult(new GetCourseItemsResponse()
			{
				CourseItems = courseItems
			});
		}
	}
}
