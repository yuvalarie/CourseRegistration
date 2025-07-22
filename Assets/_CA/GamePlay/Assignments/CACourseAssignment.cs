using _CA.GamePlay.Courses;

namespace _CA.GamePlay
{
    public class CACourseAssignment : ICAAssignment
    {
        public float Amount { get; }
        public string CourseName { get; }
        
        public bool IsTwoLectureAssignment { get; }
        public CACourse Course { get; }
        
        public CACourseAssignment(CACourse course)
        {
            Course = course;
            Amount = course.CourseDuration;
            CourseName = course.CourseName;
            IsTwoLectureAssignment = course.IsTwoLectureCourse;
        }
    }
}