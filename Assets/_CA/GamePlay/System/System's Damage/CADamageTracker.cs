using System.Collections.Generic;
using System.Linq;
using _CA.Core.BaseMono;
using _CA.GamePlay.Courses;
using UnityEngine;

namespace _CA.GamePlay
{
    public class CADamageTracker
    {
        private List<ICAAssignment> damageHistory = new List<ICAAssignment>();
        private HashSet<ICAAssignment> recordedAssignments = new();

        public void RecordDamage(ICAAssignment damage)
        {
            if (recordedAssignments.Contains(damage)) return;
            recordedAssignments.Add(damage);
            damageHistory.Add(damage);
            Debug.Log("damage list count " + damageHistory.Count);
            Debug.Log("damage "+ GetTotalDamage());
            Debug.Log("damage list " + damageHistory);
            if (damage.IsTwoLectureAssignment) damageHistory.Add(damage);
        }
        
        public float GetTotalDamage() => damageHistory.Sum(d => d.Amount);
        
        public void ResetAllDamage()
        {
            foreach (var damage in damageHistory)
            {
                CACourseToUiCatalog.Instance.ResetCourseUI(damage);
            }
            damageHistory.Clear();
            recordedAssignments.Clear();
        }

        public void RemoveLastDamage()
        {
            if (damageHistory.Count > 0)
            {
                ICAAssignment lastDamage = damageHistory.Last();
                recordedAssignments.Remove(lastDamage);
                damageHistory.RemoveAt(damageHistory.Count - 1);
                CACourseToUiCatalog.Instance.ResetCourseUI(lastDamage);
                Debug.Log("Removed last damage. Remaining count: " + damageHistory.Count);
            }
        }

        public void RemoveDamage(ICAAssignment assignment)
        {
            damageHistory.Remove(assignment);
            recordedAssignments.Remove(assignment);
        }
        
        public List<ICAAssignment> DamageHistory => damageHistory;




    }
}