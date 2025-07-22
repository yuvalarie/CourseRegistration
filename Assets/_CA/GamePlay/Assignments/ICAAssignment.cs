using System;

namespace _CA.GamePlay
{
    public interface ICAAssignment
    {
        public float Amount { get; }
        
        public String CourseName { get; }
        
        public bool IsTwoLectureAssignment { get; }
    }
}