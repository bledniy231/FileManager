using FileManager.Contract.Courses;
using FileManager.Contract.Models.PianoMentor.Courses;
using FileManager.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FileManager.BLL.Couses
{
	internal class GetCoursesHandler(FileManagerDbContext dbContext) : IRequestHandler<GetCoursesRequest, GetCoursesResponse>
	{
		private readonly FileManagerDbContext _dbContext = dbContext;

		public Task<GetCoursesResponse> Handle(GetCoursesRequest request, CancellationToken cancellationToken)
		{
			var coursesUserProgresses = _dbContext.CourseUserProgresses
				.Where(cup => cup.UserId == request.UserId)
				.ToList();

			var courses = _dbContext.Courses
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

			return Task.FromResult(new GetCoursesResponse
			{
				Courses = courses
			});
		}
	}
}
