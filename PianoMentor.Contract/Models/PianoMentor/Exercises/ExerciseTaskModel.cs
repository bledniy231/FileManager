namespace PianoMentor.Contract.Models.PianoMentor.Exercises;

public class ExerciseTaskModel
{
    public int ExerciseTaskId { get; set; }
    public int CourseItemId { get; set; }
    /// <summary>
    /// Тип упражнения (сравнение, определение, несколько интервалов в одном упражнении)
    /// </summary>
    public int ExerciseTypeId { get; set; }
    public List<int> IntervalsInTaskIds { get; set; }
}