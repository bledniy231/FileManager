using PianoMentor.Contract.Models.PianoMentor.Courses;

namespace PianoMentor.BLL.WordsEndings
{
	public class WordsEndingsManager
	{
		private Dictionary<CourseItemTypesEnumeration, string[]> WordEndings { get; } =
			new Dictionary<CourseItemTypesEnumeration, string[]>()
			{
				{ CourseItemTypesEnumeration.Lecture, ["лекция", "лекции", "лекций"] },
				{ CourseItemTypesEnumeration.Exercise, ["упражнение", "упражнения", "упражнений"] },
				{ CourseItemTypesEnumeration.Quiz, ["тест", "теста", "тестов"] }
			};

		public string GetEnding(CourseItemTypesEnumeration courseItemType, int count)
		{
			count %= 100;
			if (count >= 5 && count <= 20)
			{
				return WordEndings[courseItemType][2];
			}

			count %= 10;
			return count switch
			{
				0 => WordEndings[courseItemType][2],
				1 => WordEndings[courseItemType][0],
				_ => count == 2 || count == 3 || count == 4 ? WordEndings[courseItemType][1] : WordEndings[courseItemType][2],
			};
		}
	}
}
