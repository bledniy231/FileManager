using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.Contract.Statistics;
using PianoMentor.DAL;
using PianoMentor.DAL.Models.PianoMentor.Courses;

namespace PianoMentor.BLL.UseCases.Statistics
{
    internal class SetCourseItemProgressHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetCourseItemProgressRequest, DefaultResponse>
    {
        public Task<DefaultResponse> Handle(SetCourseItemProgressRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var courseItemExists = dbContext.CourseItems.Any(ci => ci.CourseItemId == request.CourseItemId);
                if (!courseItemExists)
                {
                    return Task.FromResult(new DefaultResponse([$"Cannot find course item id with id \'{request.CourseItemId}\'"]));
                }
                
                var itemProgressDb = dbContext.CourseItemUserProgresses
                    .FirstOrDefault(ciup =>
                        ciup.UserId == request.UserId
                        && ciup.CourseItemId == request.CourseItemId);
                if (itemProgressDb == null)
                {
                    itemProgressDb = new CourseItemUserProgress
                    {
                        UserId = request.UserId,
                        CourseItemId = request.CourseItemId,
                        CreatedAt = DateTime.UtcNow
                    };
                    dbContext.CourseItemUserProgresses.Add(itemProgressDb);
                }

                itemProgressDb.CourseItemProgressTypeId =
                    Enum.TryParse(typeof(CourseItemProgressTypesEnum), request.CourseItemProgressType, out var courseItemProgressType)
                        ? (int)courseItemProgressType
                        : 0;

                if (itemProgressDb.CourseItemProgressTypeId == 0)
                {
                    return Task.FromResult(new DefaultResponse([$"Course item progress type \'{request.CourseItemProgressType}\' is not valid"]));
                }
                dbContext.SaveChanges();
                
                var countAllCourseItems = dbContext.CourseItems.Count(ci => ci.CourseId == request.CourseId);
                if (countAllCourseItems == 0)
                {
                    return Task.FromResult(new DefaultResponse([$"Course \'{request.CourseId}\' has no items"]));
                }
                
                var countCompletedCourseItems = dbContext.CourseItemUserProgresses
                    .Count(ciup =>
                        ciup.UserId == request.UserId
                        && ciup.CourseItem.CourseId == request.CourseId
                        && ciup.CourseItemProgressTypeId == (int)CourseItemProgressTypesEnum.Completed);
                int courseProgress = (int)Math.Ceiling((double)countCompletedCourseItems * 100 / countAllCourseItems);

                var courseProgressDb = dbContext.CourseUserProgresses
                    .FirstOrDefault(cup =>
                        cup.UserId == request.UserId
                        && cup.CourseId == request.CourseId);
                if (courseProgressDb == null)
                {
                    courseProgressDb = new CourseUserProgress
                    {
                        CourseId = request.CourseId,
                        UserId = request.UserId
                    };
                    dbContext.CourseUserProgresses.Add(courseProgressDb);
                }

                courseProgressDb.ProgressInPercent = courseProgress;
                dbContext.SaveChanges();

                return Task.FromResult(new DefaultResponse(null));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new DefaultResponse([ex.Message]));
            }
        }
    }
}
