namespace PianoMentor.Contract.Models.PianoMentor.Exercises;

public enum ExerciseTypesEnum
{
    /// <summary>
    /// Сравнение интервалов восходящих среди двух
    /// </summary>
    ComparisonAsc = 1,
    /// <summary>
    /// Сравнение интервалов нисходящих среди двух
    /// </summary>
    ComparisonDesc = 2,
    /// <summary>
    /// Определение интервалов восходящих среди двух
    /// </summary>
    DeterminationAsc = 3,
    /// <summary>
    /// Определение интервалов нисходящих среди двух
    /// </summary>
    DeterminationDesc = 4,
    /// <summary>
    /// Сравнение гармонических интервалов среди двух
    /// </summary>
    ComparisonHarmonious = 5,
    /// <summary>
    /// Определение гармонических интервалов среди двух
    /// </summary>
    DeterminationHarmonious = 6,
    /// <summary>
    /// Определение интервала среди нескольких интервалов 
    /// </summary>
    DeterminationMultiple = 7
}