using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Exercises;
using PianoMentor.Contract.Models.PianoMentor.Exercises;
using PianoMentor.DAL;
using PianoMentor.DAL.Models.PianoMentor.Exercises;

namespace PianoMentor.BLL.Exercises;

public class SetExercisesTasksHandler(PianoMentorDbContext dbContext) : IRequestHandler<SetExercisesTasksRequest, DefaultResponse>
{
    public Task<DefaultResponse> Handle(SetExercisesTasksRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var exerciseTasks = request.ExerciseTasks.Select(et => new ExerciseTask
            {
                CourseItemId = et.CourseItemId,
                ExerciseTypeId = (int)Enum.Parse<ExerciseTypesEnum>(et.ExerciseTypeName),
                IntervalsInTask = et.IntervalsInTaskNames.Select(n => new Interval()
                {
                    IntervalName = Enum.Parse<IntervalsEnum>(n).ToString()
                }).ToList()
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