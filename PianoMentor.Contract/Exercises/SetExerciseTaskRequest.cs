using MediatR;
using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Exercises;

namespace PianoMentor.Contract.Exercises;

public class SetExercisesTasksRequest : IRequest<DefaultResponse>
{
    public List<ExerciseTaskModel> ExerciseTasks { get; set; }
}