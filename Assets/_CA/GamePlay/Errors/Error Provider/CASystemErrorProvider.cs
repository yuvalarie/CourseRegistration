using System;
using System.Collections;
using _CA.Core.BaseMono;
using _CA.Core.Managers;
using _CA.Pools.ErrorPool;
using UnityEngine;

namespace _CA.GamePlay
{
    public class CASystemErrorProvider : CABaseMono
    {
        [SerializeField] private float errorProvideInterval = 3f;
        [SerializeField] private CAErrorPool errorPool;
        
        private CASmartErrorProvider _errorProvider;
        private CADamageTracker _damageTracker;
        private ICAError _currentError;
        
        private void Awake()
        {
            _errorProvider = new CASmartErrorProvider(errorPool);
            _damageTracker = CAServiceLocator.Instance.DamageTracker;
        }

        private void Start()
        {
            StartCoroutine(ErrorRoutine());
        }

        private IEnumerator ErrorRoutine()
        {
            while (true)
            {
                Debug.Log("ErrorRoutine");
                yield return new WaitForSeconds(errorProvideInterval);
                var error = _errorProvider.PickError(_damageTracker);
                ActivateError(error);
            }
        }

        private void SetErrorType(ICAError error)
        {
            _currentError = error;
        }

        private void ActivateError(ICAError error)
        {
            SetErrorType(error);
            Debug.Log("ActivateError " + _currentError?.GetType());
            _currentError?.Execute(_damageTracker);
        }
    }
}