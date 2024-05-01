using FileManager.Contract.Courses;
using FileManager.Contract.Default;
using FileManager.Contract.Models.PianoMentor.Courses;
using FileManager.DAL;
using FileManager.DAL.Domain.PianoMentor.Courses;
using MediatR;
using System.Diagnostics;

namespace FileManager.BLL.Couses
{
	internal class CheckIfCourseItemExistsHandler(FileManagerDbContext dbContext) : IRequestHandler<CheckIfCourseItemExistsRequest, DefaultResponse>
	{
		private readonly FileManagerDbContext _dbContext = dbContext;

		public Task<DefaultResponse> Handle(CheckIfCourseItemExistsRequest request, CancellationToken cancellationToken)
		{
			if (!Enum.IsDefined(typeof(CourseItemTypesEnumeration), request.CourseItemTypeId))
			{
				return Task.FromResult(new DefaultResponse(["Course item type id is incorrect"]));
			}

			var existsInDb = _dbContext.CourseItems.Any(x =>
				x.CourseItemId == request.CourseItemId
				&& x.CourseItemTypeId == request.CourseItemTypeId
				&& !x.IsDeleted);

			return existsInDb
				? Task.FromResult(new DefaultResponse(null))
				: Task.FromResult(new DefaultResponse([$"Course item with ID {request.CourseItemId}/TypeID {request.CourseItemTypeId} does not exist in DB"]));
		}
	}
}
