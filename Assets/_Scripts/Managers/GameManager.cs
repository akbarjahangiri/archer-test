using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Archer.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private const string MainMenuScene = "MainMenu";

        private const string GameplayScene = "Gameplay";

        public static event Action OnGameInit;
        public event Action OnNewGame;
        public static event Action<GameState> OnGameStageChanged;

        public GameState currentGameState;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            currentGameState = GameState.CharacterIdle;
            OnGameInit?.Invoke();
        }

        public void ChangeGameState(GameState newState)
        {
            if (currentGameState != newState) 
            {
                Debug.Log($"Change {currentGameState} => {newState} ");
                currentGameState = newState;
                OnGameStageChanged?.Invoke(newState);
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
            OnNewGame?.Invoke();
            LoadGameplay();
            currentGameState = GameState.CharacterIdle;
            ChangeGameState(currentGameState);
        }
    }

    public enum GameState
    {
        CharacterIdle,
        BowRotate,
        BowAim,
        BowChargeStart,
        BowChargeEnd,
        BowShoot,
        CheckArrow,
        ReloadArrow,
        Victory,
        Lose
    }
}