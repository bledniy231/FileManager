using MediatR;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Courses;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.DAL;

namespace PianoMentor.BLL.UseCases.Courses
{
	internal class GetCoursesHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetCoursesRequest, GetCoursesResponse>
	{
		public Task<GetCoursesResponse> Handle(GetCoursesRequest request, CancellationToken cancellationToken)
		{
			try
			{
				var coursesUserProgresses = dbContext.CourseUserProgresses
					.AsNoTracking()
					.Where(cup => cup.UserId == request.UserId)
					.ToList();

				var courses = dbContext.Courses
					.AsNoTracking()
					.AsEnumerable()
					.Select(c => new CourseModel
					{
						CourseId = c.CourseId,
						Position = c.Position,
						Title = c.Title,
						Subtitle = c.Subtitle,
						Description = c.Description,
						ProgressInPercent = coursesUserProgresses
							.Where(cup => cup.CourseId == c.CourseId)
							.Select(cup => cup.ProgressInPercent)
							.DefaultIfEmpty(0)
							.FirstOrDefault()
					})
					.ToList();

				return Task.FromResult(new GetCoursesResponse(courses, null));
			}
			catch (Exception ex)
			{
				return Task.FromResult(new GetCoursesResponse(null, [ex.Message, ex.StackTrace]));
			}
		}
	}
}
