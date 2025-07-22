using System;
using _CA.Core.BaseMono;
using _CA.Core.Managers;
using _CA.GamePlay.Courses;
using UnityEngine;

namespace _CA.GamePlay
{
    public class CAStudentAssignmentProvider : CABaseMono
    {
        [SerializeField] private CACourseCatalog courseCatalog;

        private CADamageTracker _damageTracker;       
        public static CAStudentAssignmentProvider Instance {get; private set;}
        
        private void Awake()
        {
            _damageTracker = CAServiceLocator.Instance.DamageTracker;
        }
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AssignCourse(ICAAssignment course)
        {
            if (course != null)
            {
                _damageTracker.RecordDamage(course);
                Debug.Log($"Assigned course: {course.CourseName} (Damage: {course.Amount})");
            }
            else
            {
                Debug.LogWarning($"Course with ID {course.CourseName} not found.");
            }
        }
        
        public void RemoveCourse(ICAAssignment course)
        {
            if (course != null)
            {
                _damageTracker.RemoveDamage(course);
                Debug.Log($"Removed course: {course.CourseName} (Damage: {course.Amount})");
                if (course.IsTwoLectureAssignment)
                    _damageTracker.RemoveDamage(course);
            }
            else
            {
                Debug.LogWarning($"Course with ID {course.CourseName} not found.");
            }
        }
    }
}