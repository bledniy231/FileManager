using PianoMentor.Contract.ApplicationUser;
using PianoMentor.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using Microsoft.AspNetCore.Components.Web;

namespace PianoMentor.BLL.ApplicationUser
{
	internal class GetUserStatisticsHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetUserStatisticsRequest, GetUserStatisticsResponse>
	{
		private readonly PianoMentorDbContext _dbContext = dbContext;

		public Task<GetUserStatisticsResponse> Handle(GetUserStatisticsRequest request, CancellationToken cancellationToken)
		{
			var coursesUserProgress = _dbContext.CourseUserProgresses
				.AsNoTracking()
				.Where(cup => cup.UserId == request.UserId)
				.Select(cup => new CourseUserProgressModel
				{
					CourseId = cup.CourseId,
					CourseName = cup.Course.Title,
					ProgressInPercent = cup.ProgressInPercent,
					UserId = request.UserId
				})
				.ToList();

			var completeCourseItemsCount = _dbContext.CourseItemUserProgresses
				.AsNoTracking()
				.Where(ciup => 
					ciup.UserId == request.UserId
					&& ciup.CourseItemProgressTypeId == (int)CourseItemProgressTypesEnumaration.Completed)
				.GroupBy(ciup => ciup.CourseItem.CourseItemTypeId)
				.Select(g => new
				{
					CourseItemTypeId = g.Key,
					Count = g.Count()
				})
				.ToList();

			var courseItemsCount = _dbContext.CourseItems
				.AsNoTracking()
				.GroupBy(ci => ci.CourseItemTypeId)
				.Select(g => new
				{
					CourseItemTypeId = g.Key,
					Count = g.Count()
				})
				.ToList();

			return Task.FromResult(
				new GetUserStatisticsResponse(
					coursesUserProgress, 
					completeCourseItemsCount.First(c => c.CourseItemTypeId == (int)CourseItemTypesEnumeration.Lecture).Count,
					completeCourseItemsCount.First(c => c.CourseItemTypeId == (int)CourseItemTypesEnumeration.Exercise).Count,
					completeCourseItemsCount.First(c => c.CourseItemTypeId == (int)CourseItemTypesEnumeration.Quiz).Count, 
					null));
		}
	}
}
