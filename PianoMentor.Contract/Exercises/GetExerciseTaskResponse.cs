using PianoMentor.Contract.Default;
using PianoMentor.Contract.Models.PianoMentor.Exercises;

namespace PianoMentor.Contract.Exercises;

public class GetExerciseTaskResponse(ExerciseTaskModel exerciseTask, string[]? errors) : DefaultResponse(errors)
{
    public ExerciseTaskModel ExerciseTask { get; set; }
}