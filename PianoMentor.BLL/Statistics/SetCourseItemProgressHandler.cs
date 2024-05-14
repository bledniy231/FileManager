using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.Contract.Statistics;
using PianoMentor.DAL;
using PianoMentor.DAL.Domain.PianoMentor.Courses;

namespace PianoMentor.BLL.Statistics
{
    internal class SetCourseItemProgressHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetCourseItemProgressRequest, DefaultResponse>
    {
        private readonly PianoMentorDbContext _dbContext = dbContext;

        public Task<DefaultResponse> Handle(SetCourseItemProgressRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var itemProgressDb = _dbContext.CourseItemUserProgresses
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
                    _dbContext.CourseItemUserProgresses.Add(itemProgressDb);
                }

                itemProgressDb.CourseItemProgressTypeId =
                    Enum.TryParse(typeof(CourseItemProgressTypesEnumaration), request.CourseItemProgressType, out var courseItemProgressType)
                        ? (int)courseItemProgressType
                        : 0;

                if (itemProgressDb.CourseItemProgressTypeId == 0)
                {
                    return Task.FromResult(new DefaultResponse([$"Course item progress type \'{request.CourseItemProgressType}\' is not valid"]));
                }
                _dbContext.SaveChanges();

                var countAllCourseItems = _dbContext.CourseItems.Count(ci => ci.CourseId == request.CourseId);
                if (countAllCourseItems == 0)
                {
                    return Task.FromResult(new DefaultResponse([$"Course \'{request.CourseId}\' has no items"]));
                }
                var countCompletedCourseItems = _dbContext.CourseItemUserProgresses
                    .Count(ciup =>
                        ciup.UserId == request.UserId
                        && ciup.CourseItem.CourseId == request.CourseId
                        && ciup.CourseItemProgressTypeId == (int)CourseItemProgressTypesEnumaration.Completed);
                int courseProgress = (int)Math.Ceiling((double)countCompletedCourseItems * 100 / countAllCourseItems);

                var courseProgressDb = _dbContext.CourseUserProgresses
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
                    _dbContext.CourseUserProgresses.Add(courseProgressDb);
                }

                courseProgressDb.ProgressInPercent = courseProgress;
                _dbContext.SaveChanges();

                return Task.FromResult(new DefaultResponse(null));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new DefaultResponse([ex.Message]));
            }
        }
    }
}
