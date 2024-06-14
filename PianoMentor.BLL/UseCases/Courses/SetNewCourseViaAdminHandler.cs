using MediatR;
using PianoMentor.Contract.Courses;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.DAL;
using PianoMentor.DAL.Models.PianoMentor.Courses;

namespace PianoMentor.BLL.UseCases.Courses
{
	internal class SetNewCourseViaAdminHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetNewCoursesViaAdminRequest, DefaultResponse>
	{
		public Task<DefaultResponse> Handle(SetNewCoursesViaAdminRequest request, CancellationToken cancellationToken)
		{
			try
			{
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
								CourseItemTypeId = (int)Enum.Parse<CourseItemTypesEnum>(ci.CourseItemType),
								Position = ci.Position
							})
							.ToList()
					})
					.ToList();

				dbContext.Courses.AddRange(courses);

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
