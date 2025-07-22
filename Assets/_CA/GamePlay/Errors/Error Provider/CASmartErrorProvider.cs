using _CA.GamePlay.Errors.Error_Mono;
using _CA.Pools.ErrorPool;
using UnityEngine;

namespace _CA.GamePlay
{
    public class CASmartErrorProvider
    {
        private readonly CAErrorPool _errorPool;
        
        public CASmartErrorProvider(CAErrorPool errorPool)
        {
            _errorPool = errorPool;
        }
        
        public ICAError PickError(CADamageTracker tracker)
        {
            var assignmentCount = tracker.DamageHistory.Count;
            Debug.Log("assignmentCount in pick error " + assignmentCount);
            
            if (assignmentCount == 0) return null;
            if (tracker.GetTotalDamage() <10) return null;
            
            var selectedError = tracker.GetTotalDamage() < 20 ? ErrorType.ResetLastError : ErrorType.ResetAllError;
            Debug.Log("selectedError "+ selectedError);
            
            var errorObj = _errorPool.Get(selectedError);
            if (errorObj == null)
            {
                Debug.LogError($"ErrorPool returned null for {selectedError}");
                return null;
            }

            var errorMono = errorObj.GetComponent<CAErrorMono>();
            if (errorMono == null)
            {
                Debug.LogError("Missing CAErrorMono component on error object");
                return null;
            }
            
            if (errorMono.GetError() == null)
            {
                Debug.LogError("errorMono.GetError() returned null");
            }
            
            return errorMono.GetError();
        }
    }
}