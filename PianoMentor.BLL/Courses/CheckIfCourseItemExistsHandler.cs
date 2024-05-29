using PianoMentor.Contract.Courses;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.DAL;
using PianoMentor.DAL.Domain.PianoMentor.Courses;
using MediatR;
using System.Diagnostics;

namespace PianoMentor.BLL.Courses
{
	internal class CheckIfCourseItemExistsHandler(PianoMentorDbContext dbContext) : IRequestHandler<CheckIfCourseItemExistsRequest, DefaultResponse>
	{
		public Task<DefaultResponse> Handle(CheckIfCourseItemExistsRequest request, CancellationToken cancellationToken)
		{
			if (!Enum.IsDefined(typeof(CourseItemTypesEnum), request.CourseItemTypeId))
			{
				return Task.FromResult(new DefaultResponse(["Course item type id is incorrect"]));
			}

			var existsInDb = dbContext.CourseItems.Any(x =>
				x.CourseItemId == request.CourseItemId
				&& x.CourseItemTypeId == request.CourseItemTypeId
				&& !x.IsDeleted);

			return existsInDb
				? Task.FromResult(new DefaultResponse(null))
				: Task.FromResult(new DefaultResponse([$"Course item with ID {request.CourseItemId}/TypeID {request.CourseItemTypeId} does not exist in DB"]));
		}
	}
}
