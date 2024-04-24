using FileManager.DAL.Domain.Identity;

namespace FileManager.DAL.Domain.PianoMentor.Courses
{
    public class CourseItemUserProgress
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int CourseItemId { get; set; }
        public int CourseItemProgressTypeId { get; set; }
        public virtual FileManagerUser User { get; set; }
        public virtual CourseItem CourseItem { get; set; }
        public virtual CourseItemProgressType CourseItemProgressType { get; set; }
    }
}
