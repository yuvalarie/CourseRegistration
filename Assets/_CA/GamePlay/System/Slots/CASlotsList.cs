using System;
using System.Collections.Generic;
using UnityEngine;

namespace _CA.GamePlay.System
{
    public class CASlotsList : MonoBehaviour
    {
        [SerializeField] private CACoruseSlot[] slots;

        private readonly Dictionary<Vector2Int, CACoruseSlot> _coordinatesSlotsDict = new Dictionary<Vector2Int, CACoruseSlot>();
        
        public static CASlotsList Instance { get; private set; }

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            foreach (var slot in slots)
            {
                _coordinatesSlotsDict[slot.SlotCoordinate] = slot;
            }
        }
        
        public CACoruseSlot GetSlot(Vector2Int coordinate)
        {
            if (_coordinatesSlotsDict.TryGetValue(coordinate, out var slot))
            {
                return slot;
            }
            Debug.LogWarning($"No slot found for coordinate ({coordinate.x}, {coordinate.y})");
            return null;
        }
    }
}