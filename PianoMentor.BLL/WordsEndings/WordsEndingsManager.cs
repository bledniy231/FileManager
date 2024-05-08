using PianoMentor.Contract.Models.PianoMentor.Courses;

namespace PianoMentor.BLL.WordsEndings
{
	public static class WordsEndingsManager
	{
		private static readonly Dictionary<CourseItemTypesEnumeration, string[]> _wordEndings  =
			new()
			{
				{ CourseItemTypesEnumeration.Lecture, ["Лекция", "Лекции", "Лекций"] },
				{ CourseItemTypesEnumeration.Exercise, ["Упражнение", "Упражнения", "Упражнений"] },
				{ CourseItemTypesEnumeration.Quiz, ["Тест", "Теста", "Тестов"] }
			};

		private static readonly string[] _wordEndingsCourse = ["Курс", "Курса", "Курсов"];

		public static string GetSimpleEnding(CourseItemTypesEnumeration courseItemType, int count)
		{
			count %= 100;
			if (count >= 5 && count <= 20)
			{
				return _wordEndings[courseItemType][2];
			}

			count %= 10;
			return count switch
			{
				0 => _wordEndings[courseItemType][2],
				1 => _wordEndings[courseItemType][0],
				_ => count == 2 || count == 3 || count == 4 ? _wordEndings[courseItemType][1] : _wordEndings[courseItemType][2],
			};
		}

		public static string? GetEndingForTextWithPlaceholders(string type, int count)
		{
			string[] wordEndingsCollection = [];
			if (type == "Course")
			{
				wordEndingsCollection = _wordEndingsCourse;
			}
			else if (type == "Quiz")
			{
				wordEndingsCollection = _wordEndings[CourseItemTypesEnumeration.Quiz];
			}
			else
			{
				return null;
			}

			count %= 100;
			if (count >= 5 && count <= 20)
			{
				return wordEndingsCollection[2].Replace(wordEndingsCollection[2][0], char.ToLower(wordEndingsCollection[2][0]));
			}

			count %= 10;
			return count switch
			{
				0 => wordEndingsCollection[2].Replace(wordEndingsCollection[2][0], char.ToLower(wordEndingsCollection[2][0])),
				1 => wordEndingsCollection[0].Replace(wordEndingsCollection[0][0], char.ToLower(wordEndingsCollection[0][0])),
				_ => count == 2 || count == 3 || count == 4 
					? wordEndingsCollection[1].Replace(wordEndingsCollection[1][0], char.ToLower(wordEndingsCollection[1][0])) 
					: wordEndingsCollection[2].Replace(wordEndingsCollection[2][0], char.ToLower(wordEndingsCollection[2][0])),
			};
		}
	}
}
