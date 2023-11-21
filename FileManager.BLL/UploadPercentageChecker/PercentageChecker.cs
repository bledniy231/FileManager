namespace FileManager.BLL.UploadPercentageChecker
{
	public class PercentageChecker : IPercentageChecker
	{
		private readonly Dictionary<long, (int FilesCountAlreadyUploaded, float PercentageCurrentFile)> _percentageValuesPerUser = [];

		public (int FilesCountAlreadyUploaded, float PercentageCurrentFile)? GetPercentage(long userId)
			=> _percentageValuesPerUser.TryGetValue(userId, out var result) ? result : null;

		public void SetPercentage(long userId, float percentage)
		{
			if (_percentageValuesPerUser.TryGetValue(userId, out var currentValue))
			{
				_percentageValuesPerUser[userId] = (currentValue.FilesCountAlreadyUploaded, percentage);
			}
			else
			{
				_percentageValuesPerUser.Add(userId, (0, percentage));
			}
		}

		public void IncreaseFilesCountAlreadyUploaded(long userId)
		{
			var currentValue = _percentageValuesPerUser[userId];
			_percentageValuesPerUser[userId] = (currentValue.FilesCountAlreadyUploaded + 1, 0f);
		}

		public void RemoveElement(long userId)
			=> _percentageValuesPerUser.Remove(userId);
	}
}
