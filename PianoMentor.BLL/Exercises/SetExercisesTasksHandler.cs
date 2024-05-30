using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Exercises;
using PianoMentor.DAL;
using PianoMentor.DAL.Models.PianoMentor.Exercises;

namespace PianoMentor.BLL.Exercises;

public class SetExercisesTasksHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetExercisesTasksRequest, DefaultResponse>
{
    public Task<DefaultResponse> Handle(SetExercisesTasksRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var requestIntervals = request.ExerciseTasks
                .SelectMany(et => et.IntervalsInTaskIds)
                .ToList();
            var neededIntervals = dbContext.Intervals
                .Where(i => requestIntervals.Contains(i.IntervalId))
                .ToList();
            
            var exerciseTasks = request.ExerciseTasks.Select(et => new ExerciseTask
            {
                CourseItemId = et.CourseItemId,
                ExerciseTypeId = et.ExerciseTypeId,
                IntervalsInTask = et.IntervalsInTaskIds.Select(it => neededIntervals.First(ni => ni.IntervalId == it)).ToList()
            }).ToList();

            dbContext.ExerciseTasks.AddRange(exerciseTasks);
            dbContext.SaveChanges();
            return Task.FromResult(new DefaultResponse(null));
        }
        catch (Exception e)
        {
            return Task.FromResult(new DefaultResponse([e.Message]));
        }
    }
}