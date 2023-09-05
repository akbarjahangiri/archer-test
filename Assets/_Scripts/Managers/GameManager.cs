using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Archer.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private const string MainMenuScene = "MainMenu";

        private const string GameplayScene = "Gameplay";

        public event Action OnNewGame;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenuScene);
        }

        public void LoadGameplay()
        {
            SceneManager.LoadScene(GameplayScene);
        }

        public void StartNewGame()
        {
            Debug.Log("Start New Game");
            OnNewGame?.Invoke();
            LoadGameplay();
        }
    }
}