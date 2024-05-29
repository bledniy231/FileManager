using PianoMentor.DAL.Domain.PianoMentor.Courses;

namespace PianoMentor.DAL.Models.PianoMentor.Exercises;

public class ExerciseTask
{
    public int ExerciseTaskId { get; set; }
    public int CourseItemId { get; set; }
    /// <summary>
    /// Тип упражнения (сравнение, определение, несколько интервалов в одном упражнении)
    /// </summary>
    public int ExerciseTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public virtual CourseItem CourseItem { get; set; }
    public virtual ExerciseType ExerciseType { get; set; }
    public virtual List<Interval> IntervalsInTask { get; set; }
}