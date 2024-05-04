namespace PianoMentor.BLL.UploadPercentageChecker
{
	public interface IPercentageChecker
	{
		public (int FilesCountAlreadyUploaded, float PercentageCurrentFile)? GetPercentage(long userId);
		public void SetPercentage(long userId, float percentage);
		public void IncreaseFilesCountAlreadyUploaded(long userId);
		public void RemoveElement(long userId);
	}
}
