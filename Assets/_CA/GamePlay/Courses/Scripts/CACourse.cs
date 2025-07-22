using UnityEngine;
using UnityEngine.UI;
using System;

namespace _CA.GamePlay.Courses
{
    [CreateAssetMenu(fileName = "CACourse", menuName = "CA/Courses/CACourse")]
    public class CACourse : ScriptableObject
    {
        [SerializeField] private int courseId;
        [SerializeField] private string courseName;
        [SerializeField] private int courseDuration;
        [SerializeField] private Sprite slotImage;
        [SerializeField] private Sprite hoverSprite;
        [SerializeField] private Vector2Int[] allowedSlotCoordinates;
        [SerializeField] private bool isTwoLectureCourse;
        [SerializeField] private Vector2Int[] twoLectureSlotCoordinates;

        public int CourseId => courseId;
        public string CourseName => courseName;
        public int CourseDuration => courseDuration;
        public Sprite SlotImage => slotImage;
        public Sprite HoverSprite => hoverSprite;
        public bool IsTwoLectureCourse => isTwoLectureCourse;
        public Vector2Int[] TwoLectureSlotCoordinates => twoLectureSlotCoordinates;

        public bool CanBeAssignedToSlot(Vector2Int slotCoordinate)
        {
            if (allowedSlotCoordinates == null || allowedSlotCoordinates.Length == 0)
                return true; // If no restrictions are set, allow all slots
                
            foreach (var t in allowedSlotCoordinates)
            {
                if (t == slotCoordinate)
                    return true;
            }
            return false;
        }
    }
}