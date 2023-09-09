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

        public GameState CurrentGameState;


        public GameState currentState = GameState.CharacterIdle;

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

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            // Set the initial game state to "CharacterIdle"
            CurrentGameState = GameState.CharacterIdle;

            // Load arrows or perform other initialization tasks
            // LoadArrows();

            // Trigger the initialization event
            OnGameInit?.Invoke();
        }

        public void ChangeGameState(GameState newState)
        {
            if (CurrentGameState != newState) // Only change if it's a different state
            {
                Debug.Log("ChangeGameState");
                CurrentGameState = newState;
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
            Debug.Log("Start New Game");
            OnNewGame?.Invoke();
            LoadGameplay();
            CurrentGameState = GameState.CharacterIdle;
            ChangeGameState(CurrentGameState);
        }
    }

    public enum GameState
    {
        CharacterIdle,
        BowRotate,
        BowCharge,
        BowShoot,
        CheckArrow,
        ReloadArrow,
        Victory,
        Lose
    }
}