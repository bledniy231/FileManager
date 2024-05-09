using MediatR;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Courses;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.DAL;

namespace PianoMentor.BLL.Couses
{
	internal class GetCourseItemsHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetCourseItemsRequest, GetCourseItemsResponse>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;

		public Task<GetCourseItemsResponse> Handle(GetCourseItemsRequest request, CancellationToken cancellationToken)
		{
			try
			{
				var courseItemsUserProgressesNames = _dbContext.CourseItemUserProgresses
					.AsNoTracking()
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
					.AsNoTracking()
					.Where(ci => ci.CourseId == request.CourseId)
					.Select(ci => new
					{
						ci.CourseItemId,
						ci.Position,
						ci.Title,
						ci.CourseId,
						ci.CourseItemType
					})
					.AsEnumerable()
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


				return Task.FromResult(new GetCourseItemsResponse(courseItems, null));
			}
			catch (Exception ex)
			{
				return Task.FromResult(new GetCourseItemsResponse(null, [ex.Message, ex.StackTrace]));
			}
		}
	}
}
