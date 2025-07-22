using System.Collections.Generic;
using System.Linq;
using _CA.Core.BaseMono;
using _CA.GamePlay.System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace _CA.GamePlay.Courses
{
    public class CAUICourseItem : CABaseMono, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CACourse courseData;
        [SerializeField] private Transform courseListParent;
        [SerializeField] private CAUICourseOptions[] courseOptions;
        
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Transform _originalParent;
        private Image _image;
        private Sprite _originalSprite;
        private Vector3 _originalPosition;
        private Vector3 _originalScale;
        private ICAAssignment _courseAssignment;
        
        private List<CACoruseSlot> _occupiedSlots = new List<CACoruseSlot>();

        public CACourse CourseData { get; private set; }
        
        public ICAAssignment CourseAssignment => _courseAssignment;

        public Sprite CourseImage => CourseData.SlotImage;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            
            if (_image != null)
            {
                _image.raycastTarget = true;
            }
        }
        
        private void Start()
        {
            CourseData = courseData;
            _courseAssignment = new CACourseAssignment(courseData);
            
            HandleOpeningSprite();
            
            _originalParent = transform.parent;
            StoreCurrentPositionAsOriginal();
            
            CACourseToUiCatalog.Instance.Register(_courseAssignment, this);
        }

        private void HandleOpeningSprite()
        {
            if (_image != null)
            {
                _originalSprite = _image.sprite;
            }
            else
            {
                Debug.LogError("Image component not found on the course item.");
                _image = gameObject.AddComponent<Image>();
                _image.raycastTarget = true;
            }
        }

        private void StoreCurrentPositionAsOriginal()
        {
            if (_rectTransform == null) return;
            _originalPosition = _rectTransform.localPosition;
            _originalScale = transform.localScale;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            transform.SetParent(transform.root);
            _canvasGroup.blocksRaycasts = false;
            ChangeSprite(courseData.HoverSprite);
            Debug.Log($"Begin Drag - Original Parent: {_originalParent.name}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                RectTransform parentRect = transform.parent as RectTransform;
                if (parentRect != null)
                {
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            parentRect,
                            eventData.position,
                            eventData.pressEventCamera,
                            out var position))
                    {
                        transform.localPosition = position;
                    }
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            
            var targetSlot = eventData.pointerEnter?.GetComponent<CACoruseSlot>();
            if (targetSlot == null)
            {
                Debug.Log("No valid slot found, resetting position");
                ResetPosition();
            }
            
            Debug.Log($"End Drag - Raycast status: {_canvasGroup.blocksRaycasts}, COURSE: {gameObject.name}, Parent: {transform.parent.name}");
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (transform.parent == courseListParent)
                ChangeSprite(courseData.HoverSprite);
            EnableUiOptions(true);
            Debug.Log($"Raycast status: {_canvasGroup.blocksRaycasts}, COURSE: {gameObject.name}, Parent: {transform.parent.name}");
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (transform.parent == courseListParent)
                ChangeSprite(_originalSprite);
            EnableUiOptions(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_occupiedSlots is not { Count: > 0 }) return;
            Debug.Log($"Course {gameObject.name} clicked while in slot - resetting position");
            ResetPosition();
            ResetScore();

        }

        private void ResetScore()
        {
            CAStudentAssignmentProvider.Instance.RemoveCourse(_courseAssignment);
        }

        public void AssignToSlots(List<CACoruseSlot> slots)
        {
            _occupiedSlots = slots;
            if (slots is { Count: > 0 })
            {
                transform.SetParent(slots[0].transform);
            }
        }

        public void ResetPosition()
        {
            _rectTransform.DOKill();
            _canvasGroup.blocksRaycasts = true;
            var worldPos = transform.position;
            
            ClearOccupiedSlots();
            _occupiedSlots.Clear();
            ResetTwoLectureCourse();
            
            if (_originalParent == null)
            {
                _originalParent = courseListParent;
                StoreCurrentPositionAsOriginal();
            }
            
            transform.SetParent(_originalParent);
            Debug.Log($"Resetting position of {gameObject.name} to parent: {transform.parent.name}, Raycast status: {_canvasGroup.blocksRaycasts}");
            
            transform.position = worldPos;
            transform.localScale = _originalScale;
            ChangeSprite(_originalSprite);
            transform.DOLocalMove(_originalPosition, 0.3f).SetEase(Ease.OutQuad);
            
        }

        private void ClearOccupiedSlots()
        {
            foreach (var slot in _occupiedSlots.Where(slot => slot != null))
            {
                slot.ClearSlot();
            }
        }

        private void ResetTwoLectureCourse()
        {
            if (!courseData.IsTwoLectureCourse) return;
            var coordinates = courseData.TwoLectureSlotCoordinates;
            foreach (var coor in coordinates)
            {
                var slot = CASlotsList.Instance.GetSlot(coor);
                if (slot != null)
                {
                    slot.ClearSlot();
                }
                else
                {
                    Debug.LogError($"Slot not found for coordinates: {coor}");
                }
            }
        }

        public void ChangeSprite(Sprite newSprite)
        {
            if (_image != null)
            {
                _image.sprite = newSprite;
                _image.raycastTarget = true;
            }
            else
            {
                Debug.LogError("Image component not found for changing sprite.");
            }
        }
        
        private void EnableUiOptions(bool enable)
        {
            foreach (var option in courseOptions)
            {
                if (option != null)
                {
                    option.EnableImage(enable);
                }
                else
                {
                    Debug.LogError("Course option is null.");
                }
            }
        }
    }
}