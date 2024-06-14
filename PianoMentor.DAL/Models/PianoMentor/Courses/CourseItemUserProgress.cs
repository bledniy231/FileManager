using PianoMentor.DAL.Models.Identity;

namespace PianoMentor.DAL.Models.PianoMentor.Courses
{
    public class CourseItemUserProgress
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int CourseItemId { get; set; }
        public int CourseItemProgressTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual PianoMentorUser User { get; set; }
        public virtual CourseItem CourseItem { get; set; }
        public virtual CourseItemProgressType CourseItemProgressType { get; set; }
    }
}
