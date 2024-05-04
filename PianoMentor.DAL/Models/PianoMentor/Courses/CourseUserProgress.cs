using PianoMentor.DAL.Domain.Identity;

namespace PianoMentor.DAL.Domain.PianoMentor.Courses
{
    public class CourseUserProgress
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int CourseId { get; set; }
        public int ProgressInPercent { get; set; }
        public virtual PianoMentorUser User { get; set; }
        public virtual Course Course { get; set; }
    }
}
