﻿namespace FileManager.DAL.Domain.PianoMentor
{
	public class CourseItem
	{
		public int CourseItemId { get; set; }
		public uint Position { get; set; }
		public string Title { get; set; }
		public int CourseTypeId { get; set; }
		public int CourseId { get; set; }
		public CourseType CourseType { get; set; }
		public Course Course { get; set; }
	}
}
