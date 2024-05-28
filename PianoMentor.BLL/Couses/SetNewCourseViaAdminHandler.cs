using PianoMentor.Contract.Courses;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.DAL;
using PianoMentor.DAL.Domain.PianoMentor.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PianoMentor.BLL.Couses
{
	internal class SetNewCourseViaAdminHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetNewCoursesViaAdminRequest, DefaultResponse>
	{
		public Task<DefaultResponse> Handle(SetNewCoursesViaAdminRequest request, CancellationToken cancellationToken)
		{
			var courseItemsTypes = Enum.GetValues<CourseItemTypesEnum>();
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

			dbContext.Courses.AddRange(courses);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return Task.FromResult(new DefaultResponse([e.Message]));
			}

			return Task.FromResult(new DefaultResponse(null));
		}
	}
}
