﻿namespace FileManager
{
	public static class BllExtensions
	{
		public static FileInfo GetInternalFile(this DAL.Domain.DataSet.BinaryData binaryData)
		{
			ArgumentNullException.ThrowIfNull(binaryData);
			ArgumentNullException.ThrowIfNull(binaryData.DataSet);
			ArgumentNullException.ThrowIfNull(binaryData.DataSet.Storage);

			string path = Path.Combine(binaryData.DataSet.Storage.BasePath,
				(binaryData.DataSetId / 1000 / 1000).ToString("d3"),
				(binaryData.DataSetId / 1000 % 1000).ToString("d3"),
				(binaryData.DataSetId % 1000).ToString("d3"),
				binaryData.Filename);

			return new FileInfo(path);
		}

		public static string GetDataSetDirectory(this DAL.Domain.DataSet.DataSet dataSet)
		{
			string path = Path.Combine(dataSet.Storage.BasePath,
				(dataSet.Id / 1000 / 1000).ToString("d3"),
				(dataSet.Id / 1000 % 1000).ToString("d3"),
				(dataSet.Id % 1000).ToString("d3"));

			return path;
		}
	}
}
