using System.Collections.Generic;
using _CA.GamePlay.Courses;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _CA.GamePlay.System
{
    public class CACoruseSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private RectTransform dropTarget;
        [SerializeField] private CACoruseSlot nextSlot;
        [SerializeField] private Vector2Int slotCoordinate;
        
        private ICAAssignment _assignedCourse;
        
        private Image _image;
        private bool IsOccupied { get; set; }
        public Vector2Int SlotCoordinate => slotCoordinate;
        
        private void Start()
        {
            _image = GetComponent<Image>();
            if (_image == null)
            {
                Debug.LogError("Image component not found on the course slot.");
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedItem = eventData.pointerDrag?.GetComponent<CAUICourseItem>();
            if (draggedItem == null) return;
            
            if (IsOccupied)
            {
                Debug.LogWarning("Slot already has a course assigned.");
                draggedItem.ResetPosition();
                return;
            }
            
            var course = draggedItem.CourseAssignment;
            
            if (!CheckAvailability(course, draggedItem))
            {
                Debug.Log($"Failed to assign course {draggedItem.name} to slot at ({slotCoordinate.x},{slotCoordinate.y})");
                draggedItem.ResetPosition();
            }
        }

        private bool CheckAvailability(ICAAssignment course, CAUICourseItem draggedItem)
        {
            if (!draggedItem.CourseData.CanBeAssignedToSlot(slotCoordinate))
            {
                Debug.LogWarning($"Course {draggedItem.CourseData.CourseName} cannot be assigned to slot at ({slotCoordinate.x},{slotCoordinate.y})");
                return false;
            }
            
            if (!CheckRequiredSlotsAvailability(course, draggedItem, out var requiredSlots)) return false;

            AssignCourseToSlots(draggedItem, requiredSlots);
            
            draggedItem.AssignToSlots(requiredSlots);
            
            LocateUi(draggedItem);
            
            AssignTwoLectureCourse(draggedItem, course);
            
            return true;
        }

        private static void AssignCourseToSlots(CAUICourseItem draggedItem, List<CACoruseSlot> requiredSlots)
        {
            foreach (var slot in requiredSlots)
            {
                slot.AssignCourse(draggedItem.CourseAssignment, draggedItem);
            }
        }

        private bool CheckRequiredSlotsAvailability(ICAAssignment course, CAUICourseItem draggedItem, out List<CACoruseSlot> requiredSlots)
        {
            requiredSlots = new List<CACoruseSlot> { this };
            var current = this;
            
            for (int i = 1; i < course.Amount; i++)
            {
                current = current.nextSlot;
                if (current == null)
                {
                    Debug.LogWarning("Not enough consecutive slots available.");
                    return false;
                }
                if (current.IsOccupied)
                {
                    Debug.LogWarning("One of the required slots is already occupied.");
                    return false;
                }
                if (!draggedItem.CourseData.CanBeAssignedToSlot(current.slotCoordinate))
                {
                    Debug.LogWarning($"Course {draggedItem.CourseData.CourseName} cannot be assigned to slot at ({current.slotCoordinate.x},{current.slotCoordinate.y})");
                    return false;
                }
                requiredSlots.Add(current);
            }

            return true;
        }

        private void AssignTwoLectureCourse(CAUICourseItem draggedItem, ICAAssignment course)
        {
            if (!draggedItem.CourseData.IsTwoLectureCourse) return;
            var twoLectureSlots = draggedItem.CourseData.TwoLectureSlotCoordinates;
            
            if (twoLectureSlots == null )
            {
                Debug.LogWarning("Two lecture course requires at least two slots.");
                return;
            }
            
            foreach (var coordinate in twoLectureSlots)
            {
                var slot = CASlotsList.Instance.GetSlot(coordinate);
                if (slot == null)
                {
                    Debug.LogWarning($"No slot found for coordinate ({coordinate.x},{coordinate.y})");
                    continue;
                }
                if (slot.IsOccupied)
                {
                    Debug.LogWarning($"Slot at ({coordinate.x},{coordinate.y}) is already occupied.");
                    continue;
                }
                slot.AssignCourse(course, draggedItem);
            }
        }

        private void LocateUi(CAUICourseItem draggedItem)
        {
            draggedItem.transform.SetParent(transform, false);
            var t = draggedItem.transform;
            t.localScale = Vector3.one;
            t.localRotation = Quaternion.identity;
            draggedItem.GetComponent<RectTransform>().anchoredPosition = dropTarget.anchoredPosition;
            t.localPosition = Vector3.zero;
            draggedItem.ChangeSprite(draggedItem.CourseImage);
        }

        private void AssignCourse(ICAAssignment course, CAUICourseItem draggedItem)
        {
            _assignedCourse = course;
            IsOccupied = true;
            var courseImage = draggedItem.CourseImage;
            ChangeSprite(courseImage);
            CAStudentAssignmentProvider.Instance.AssignCourse(course);
            Debug.Log($"Dropped course: {course.CourseName} to slot: {name}");
        }
        
        public void ClearSlot()
        {
            IsOccupied = false;
            _assignedCourse = null;
            ChangeSprite(null);
        }
        
        private void ChangeSprite(Sprite newSprite)
        {
            if (_image != null)
            {
                _image.sprite = newSprite;
            }
            else
            {
                Debug.LogError("Image component not found for changing sprite.");
            }
        }
    }
}