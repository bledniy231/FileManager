namespace FileManager.DAL.Domain.PianoMentor.Courses
{
    public class CourseItem
    {
        public int CourseItemId { get; set; }
        public int Position { get; set; }
        public string Title { get; set; }
        public int CourseTypeId { get; set; }
        public int CourseId { get; set; }
        public virtual CourseItemType CourseItemType { get; set; }
        public virtual Course Course { get; set; }
    }
}
