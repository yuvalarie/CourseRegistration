using System;
using _CA.Core.BaseMono;
using _CA.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _CA.Core.Managers
{
    public class CADamageManager : CABaseMono
    {
        [SerializeField] private float targetDamage;
        [SerializeField] private TMP_Text damageText;
        
        private CADamageTracker _damageTracker;

        private bool GameOver { get; set; }
        private static CADamageManager Instance {get; set;}
        
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
            _damageTracker = CAServiceLocator.Instance.DamageTracker;
            GameOver = false;
        }

        private void Update()
        {
            if (GameOver) return;

            if (_damageTracker.GetTotalDamage() >= targetDamage)
            {
                GameWon();
            }

            if (damageText != null)
            {
                damageText.text = $"{targetDamage - _damageTracker.GetTotalDamage()}";
            }
        }

        private void GameWon()
        {
            GameOver = true;
            CATimeManager.Instance.GameOver = true;
            Debug.Log("Game Over: You have won!");
            EventManager.GameWon();
        }
        
    }
}