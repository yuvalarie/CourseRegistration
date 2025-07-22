using System;
using _CA.Core.BaseMono;

namespace _CA.Core.Managers
{
    public class EventManager : CABaseMono
    {
        private static EventManager Instance {get; set;}

        private void Start()
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

        public static event Action OnGameWon;
        
        public static void GameWon()
        {
            OnGameWon?.Invoke();
        }
        
        public static event Action OnGameLost;
        
        public static void GameLost()
        {
            OnGameLost?.Invoke();
        }
        
        public static event Action OnStartPressed;
        
        public static void StartPressed()
        {
            OnStartPressed?.Invoke();
        }
        
        
    }
}