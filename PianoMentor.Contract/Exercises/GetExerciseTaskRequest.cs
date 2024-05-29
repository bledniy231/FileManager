using MediatR;

namespace PianoMentor.Contract.Exercises;

public class GetExerciseTaskRequest(int courseItemId) : IRequest<GetExerciseTaskResponse>
{
    public int CourseItemId { get; set; } = courseItemId;
}