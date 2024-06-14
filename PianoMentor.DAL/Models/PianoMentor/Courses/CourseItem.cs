namespace PianoMentor.DAL.Models.PianoMentor.Courses
{
    public class CourseItem
    {
        public int CourseItemId { get; set; }
        public int Position { get; set; }
        public string Title { get; set; }
        public int CourseItemTypeId { get; set; }
        public int CourseId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? AttachedDataSetId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Models.DataSet.DataSet AttachedDataSet { get; set; }
        public virtual CourseItemType CourseItemType { get; set; }
        public virtual Course Course { get; set; }
    }
}
