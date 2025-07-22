using System.Collections.Generic;
using UnityEngine;

namespace _CA.GamePlay.Courses
{
    [CreateAssetMenu(fileName = "CourseCatalog", menuName = "CA/Course Catalog")]
    public class CACourseCatalog : ScriptableObject
    {
        [SerializeField] private List<Courses.CACourse> courses;
        
        public CACourse GetCourseById(int courseId)
        {
            return courses.Find(c=> c.CourseId == courseId);
        }
        
        public List<CACourse> GetAllCourses()
        {
            return courses;
        }
    }
}