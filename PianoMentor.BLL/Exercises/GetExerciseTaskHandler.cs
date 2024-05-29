using MediatR;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Exercises;
using PianoMentor.Contract.Models.PianoMentor.Exercises;
using PianoMentor.DAL;

namespace PianoMentor.BLL.Exercises;

public class GetExerciseTaskHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetExerciseTaskRequest, GetExerciseTaskResponse>
{
    public Task<GetExerciseTaskResponse> Handle(GetExerciseTaskRequest request, CancellationToken cancellationToken)
    {
        var exerciseTask = dbContext.ExerciseTasks
            .Include(x => x.ExerciseType)
            .Include(x => x.IntervalsInTask)
            .FirstOrDefault(x => x.CourseItemId == request.CourseItemId && !x.IsDeleted);

        if (exerciseTask == null)
        {
            return Task.FromResult(new GetExerciseTaskResponse(null, ["Exercise task not found"]));
        }

        var exerciseTaskModel = new ExerciseTaskModel
        {
            ExerciseTaskId = exerciseTask.ExerciseTaskId,
            CourseItemId = exerciseTask.CourseItemId,
            ExerciseTypeName = exerciseTask.ExerciseType.ExerciseTypeName,
            IntervalsInTaskNames = exerciseTask.IntervalsInTask.Select(x => x.IntervalName).ToList()
        };

        return Task.FromResult(new GetExerciseTaskResponse(exerciseTaskModel, null));
    }
}