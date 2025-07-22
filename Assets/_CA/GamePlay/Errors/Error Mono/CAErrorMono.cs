using _CA.Core.BaseMono;
using UnityEngine;

namespace _CA.GamePlay.Errors.Error_Mono
{
    public class CAErrorMono : CABaseMono
    {
        [SerializeField] private ErrorType errorType;

        private ICAError _error;
        
        private void OnEnable()
        {
            if (_error == null)
            {
                _error = ErrorTypeFactory.EnumToErrorType(errorType);
            }
        }
        
        public ICAError GetError()
        {
            if (_error == null)
            {
                Debug.LogWarning("Error was null in GetError — initializing now.");
                _error = ErrorTypeFactory.EnumToErrorType(errorType);
            }

            return _error;
        }
        
    }
}