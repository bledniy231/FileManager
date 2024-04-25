using FileManager.Contract.Courses;
using FileManager.Contract.Default;
using FileManager.Contract.Models.PianoMentor.Courses;
using FileManager.DAL;
using FileManager.DAL.Domain.PianoMentor.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.BLL.Couses
{
	internal class SetNewCourseViaAdminHandler(FileManagerDbContext dbContext) : IRequestHandler<SetNewCoursesViaAdminRequest, DefaultResponse>
	{
		private readonly FileManagerDbContext _dbContext = dbContext;

		public Task<DefaultResponse> Handle(SetNewCoursesViaAdminRequest request, CancellationToken cancellationToken)
		{
			var courseItemsTypes = Enum.GetValues<CourseItemTypesEnumeration>();
			if (courseItemsTypes == null || courseItemsTypes.Length == 0)
			{
				return Task.FromResult(new DefaultResponse(["Course item types not found"]));
			}

			var courses = request.CoursesModels
				.Select(c => new Course
				{
					Title = c.Title,
					Subtitle = c.Subtitle,
					Description = c.Description,
					Position = c.Position,
					CourseItems = c.CourseItemModels
						.Select(ci => new CourseItem
						{
							Title = ci.Title,
							CourseItemTypeId = (int)courseItemsTypes.FirstOrDefault(t => t.ToString() == ci.CourseItemType),
							Position = ci.Position
						})
						.ToList()
				})
				.ToList();

			_dbContext.Courses.AddRange(courses);

			try
			{
				_dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return Task.FromResult(new DefaultResponse([e.Message]));
			}

			return Task.FromResult(new DefaultResponse(null));
		}
	}
}
