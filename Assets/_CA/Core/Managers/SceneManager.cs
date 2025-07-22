using UnityEngine;

namespace _CA.Core.Managers
{
    public class SceneManager : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        
        private void OnEnable()
        {
            EventManager.OnStartPressed += LoadGameScene;
            EventManager.OnGameWon += LoadGameWonScene;
            EventManager.OnGameLost += LoadGameLostScene;
        }
        
        private void OnDisable()
        {
            EventManager.OnStartPressed -= LoadGameScene;
            EventManager.OnGameWon -= LoadGameWonScene;
            EventManager.OnGameLost -= LoadGameLostScene;
        }
        
        public void LoadGameScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("BossLevel");
        }

        public void LoadGameLostScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndSceneLose");
        }
        
        public void LoadGameWonScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndSceneWin");
        }
    }
}