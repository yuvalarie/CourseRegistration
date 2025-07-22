using _CA.Core.BaseMono;
using TMPro;
using UnityEngine;

namespace _CA.Core.Managers
{
    public class CATimeManager : CABaseMono
    {
        [SerializeField] private float gameDuration;
        [SerializeField] private TMP_Text timerText;

        private float _remainingTime;
        
        public bool GameOver { get; set; }
        public static CATimeManager Instance {get; private set;}

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
            _remainingTime = gameDuration;
            GameOver = false;
        }

        private void Update()
        {
            if (GameOver) return;

            _remainingTime -= Time.deltaTime;
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(_remainingTime / 60f);
                int seconds = Mathf.FloorToInt(_remainingTime % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
            if (_remainingTime <= 1)
            {
                TimeOver();
            }
        }

        private void TimeOver()
        {
            GameOver = true;
            Instance.GameOver = true;
            Debug.Log("Game Over! Time's up!");
            EventManager.GameLost();
        }
    }
}