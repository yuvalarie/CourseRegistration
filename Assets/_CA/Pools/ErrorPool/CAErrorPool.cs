using System.Collections.Generic;
using _CA.Core.BaseMono;
using _CA.GamePlay;
using _CA.GamePlay.Errors.Error_Mono;
using Unity.VisualScripting;
using UnityEngine;

namespace _CA.Pools.ErrorPool
{
    [System.Serializable]
    public class ErrorPrefabEntry
    {
        public ErrorType errorType;
        public CAErrorMono prefab;
    }
    
    public class CAErrorPool : CABaseMono
    {
        [SerializeField] private List<ErrorPrefabEntry> prefabEntries;
        [SerializeField] private Transform errorParent; 
        
        private Dictionary<ErrorType, Queue<GameObject>> _pools;
        private Dictionary<ErrorType, GameObject> _prefabLookup;
        
        private void Awake()
        {
            _pools = new Dictionary<ErrorType, Queue<GameObject>>();
            _prefabLookup = new Dictionary<ErrorType, GameObject>();

            foreach (var entry in prefabEntries)
            {
                _prefabLookup[entry.errorType] = entry.prefab.gameObject;
                _pools[entry.errorType] = new Queue<GameObject>();
            }
        }
        
        public GameObject Get(ErrorType type)
        {
            if (!_pools.ContainsKey(type))
            {
                Debug.LogError($"No pool exists for ErrorType: {type}");
                return null;
            }

            if (_pools[type].Count > 0)
            {
                var obj = _pools[type].Dequeue();
                obj.SetActive(true);
                return obj;
            }

            var newObj = Instantiate(_prefabLookup[type], errorParent);
            return newObj;
        }
        
        public void ReturnToPool(GameObject errorObject, ErrorType type)
        {
            if (!_pools.ContainsKey(type))
            {
                Destroy(errorObject);
                Debug.LogWarning($"Trying to return object to non-existent pool: {type}");
                return;
            }

            errorObject.SetActive(false);
            _pools[type].Enqueue(errorObject);
        }
    }
}