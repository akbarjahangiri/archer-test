using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Archer.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] private HudManager _hudManager;
        public LevelData[] levelDataArray;
        private LevelData currentLevelData;
        public int currentLevelDataNumber;
        private PlayerProgress playerProgress;
        private PlayerScore playerScore; // Updated to PlayerScore
        private int _playerLevelScore = 0;
        private const string MainMenuScene = "MainMenu";
        private const string GameplayScene = "Gameplay";
        public static event Action OnGameInit;
        public event Action OnNewGame;

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
                return;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == GameplayScene)
            {
                _hudManager = FindObjectOfType<HudManager>();
                _hudManager.UpdateLevelInfo(currentLevelData.levelNumber.ToString(),
                    currentLevelData.requiredScore.ToString());

                if (_hudManager == null)
                {
                    Debug.LogError("HudManager not found in the Gameplay scene.");
                }
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
            currentLevelData = levelDataArray[0];
            playerProgress = new PlayerProgress();
            currentLevelDataNumber = levelDataArray[0].levelNumber;
            playerProgress.currentLevel = 1;
            LoadGameplay();
        }

        public void LoadGame()
        {
            LoadPlayerProgressFromJSON();
        }

        public void HandleGameVictory()
        {
            SavePlayerProgress();
        }

        public void ExitLevel()
        {
            LoadMainMenu();
        }

        public void AddScore(int score)
        {
            _playerLevelScore += score;
            playerProgress.score += _playerLevelScore;
            _hudManager.UpdateScore(_playerLevelScore.ToString());
        }

        #region StateHandles

        public bool CheckLastLevel()
        {
            if (currentLevelData.levelNumber == levelDataArray.Length)
            {
                return true;
            }

            return false;
        }

        public void LoadNextLevel()
        {
            SavePlayerProgress();
            int nextLevelNumber = currentLevelData.levelNumber + 1;

            if (nextLevelNumber >= 1 && nextLevelNumber <= levelDataArray.Length)
            {
                currentLevelData = levelDataArray[nextLevelNumber - 1];
                playerProgress.currentLevel = currentLevelData.levelNumber;
                LoadGameplay();
            }
            else
            {
                Debug.LogError("Invalid next level number.");
            }
        }

        public void CompleteLevel(int levelNumber, int score)
        {
            if (levelNumber == playerProgress.currentLevel)
            {
                playerProgress.score = score;
                if (score >= GetRequiredScoreForLevel(levelNumber))
                {
                    playerProgress.currentLevel++;
                    if (playerProgress.currentLevel <= levelDataArray.Length)
                    {
                        // Transition to the next level or handle level completion
                        // Example: Transition to level playerProgress.currentLevel
                    }
                    else
                    {
                        // Player completed all levels
                        // Handle game completion or victory state
                    }
                }

                SavePlayerProgress();
            }
        }

        #endregion

        #region PlayerProgress

        private void LoadPlayerProgress()
        {
            string filePath = Application.dataPath + Path.AltDirectorySeparatorChar + "/playerProgress.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                playerProgress = JsonUtility.FromJson<PlayerProgress>(json);
            }
            else
            {
                playerProgress = new PlayerProgress();
                SavePlayerProgress();
            }
        }

        public void SavePlayerProgress()
        {
            Debug.Log("Save Progress");
            string json = JsonUtility.ToJson(playerProgress);
            string filePath = Application.dataPath + Path.AltDirectorySeparatorChar + "/playerProgress.json";
            File.WriteAllText(filePath, json);
        }

        public int GetRequiredScoreForLevel(int levelNumber)
        {
            if (levelNumber >= 1 && levelNumber <= levelDataArray.Length)
            {
                return levelDataArray[levelNumber - 1].requiredScore;
            }

            return 0;
        }

        public void LoadPlayerProgressFromJSON()
        {
            string filePath = Application.persistentDataPath + "/playerProgress.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                playerProgress = JsonUtility.FromJson<PlayerProgress>(json);
            }
            else
            {
                playerProgress = new PlayerProgress();
            }
        }

        public bool CheckPlayerProgress()
        {
            if (_playerLevelScore >= currentLevelData.requiredScore)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region PlayerScore

        public void AddPlayerScore(string playerName, int score)
        {
            PlayerScoreEntry newScore = new PlayerScoreEntry
            {
                name = playerName,
                score = score
            };
            playerScore.scores.Add(newScore);
            SavePlayerScores();
        }

        private void SavePlayerScores()
        {
            string filePath = Application.persistentDataPath + "/playerScores.json";
            string json = JsonUtility.ToJson(playerScore);
            File.WriteAllText(filePath, json);
        }

        public List<PlayerScoreEntry> GetPlayerScores()
        {
            return playerScore.scores;
        }

        private void LoadPlayerScores()
        {
            string filePath = Application.persistentDataPath + "/playerScores.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                playerScore = JsonUtility.FromJson<PlayerScore>(json);
            }
            else
            {
                playerScore = new PlayerScore();
                SavePlayerScores();
            }
        }

        #endregion
    }
}