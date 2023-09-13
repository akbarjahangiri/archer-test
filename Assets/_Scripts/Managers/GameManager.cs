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
        private PlayerScores _playerScores;
        private static string savePath;
        private static string scoreSavePath;

        private int _playerOverallLevelScore = 0;
        private int _playerCurrentLevelScore = 0;

        private const string MainMenuScene = "MainMenu";
        private const string GameplayScene = "Gameplay";

        public static event Action OnGameInit;
        // public event Action OnNewGame;

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

            savePath = Application.dataPath + Path.AltDirectorySeparatorChar + "/playerProgress.json";
            scoreSavePath = Application.dataPath + Path.AltDirectorySeparatorChar + "/playerScores.json";
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == GameplayScene)
            {
                _hudManager = FindObjectOfType<HudManager>();
                _hudManager.UpdateLevelInfo(currentLevelData, playerProgress);
                _playerCurrentLevelScore = 0;
                if (_hudManager == null)
                {
                    Debug.LogError("HudManager not found in the Gameplay scene.");
                }
            }
        }

        public int GetCurrentLevelNumber()
        {
            return currentLevelData.levelNumber;
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
            LoadGameplay();
        }

        public void HandleGameVictory()
        {
            _hudManager.ShowVictoryPanel();
            //SavePlayerProgress();
        }

        public void ExitLevel()
        {
            LoadMainMenu();
        }

        public void AddScore(int score)
        {
            _playerOverallLevelScore += score;
            _playerCurrentLevelScore += score;

            _hudManager.UpdateScore(_playerOverallLevelScore.ToString());
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
            int nextLevelNumber = currentLevelData.levelNumber + 1;
            playerProgress.score = _playerOverallLevelScore;
            playerProgress.currentLevel = nextLevelNumber;
            SavePlayerProgress();

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

        public bool CheckPlayerSave()
        {
            if (IsJsonFileEmpty(savePath))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region PlayerProgress

        public void SavePlayerProgress()
        {
            string json = JsonUtility.ToJson(playerProgress);
            string directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(savePath, json);
            Debug.Log("Save Progress");
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
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                playerProgress = JsonUtility.FromJson<PlayerProgress>(json);
                currentLevelData = levelDataArray[playerProgress.currentLevel - 1];
            }
            else
            {
                playerProgress = new PlayerProgress();
            }
        }

        public bool CheckPlayerProgress()
        {
            if (_playerCurrentLevelScore >= currentLevelData.requiredScore)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region PlayerScore

        public void AddPlayerScore(string playerName)
        {
            _playerScores = LoadPlayerScores();
            PlayerScoreEntry newScore = new PlayerScoreEntry
            {
                name = playerName,
                score = playerProgress.score
            };
            Debug.Log("Player Name: " + playerName);
            Debug.Log("score: " + playerProgress.score);
            _playerScores.scores.Add(newScore);
            SavePlayerScores();
        }

        private void SavePlayerScores()
        {
            string json = JsonUtility.ToJson(_playerScores);
            File.WriteAllText(scoreSavePath, json);
            // Clear PlayerProgres 
            ClearJsonFile(savePath);
        }

        public List<PlayerScoreEntry> GetPlayerScores()
        {
            return _playerScores.scores;
        }

        public PlayerScores LoadPlayerScores()
        {
            if (File.Exists(scoreSavePath))
            {
                string json = File.ReadAllText(scoreSavePath);
                var playerScores = JsonUtility.FromJson<PlayerScores>(json);
                return playerScores;
            }
            else
            {
                var playerScores = new PlayerScores();
                return playerScores;

                //  SavePlayerScores();
            }
        }

        #endregion

        public void ClearJsonFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("JSON file cleared successfully.");
            }
            else
            {
                Debug.LogWarning("JSON file not found. Nothing to clear.");
            }
        }

        public bool IsJsonFileEmpty(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return string.IsNullOrEmpty(json) || json.Trim() == "{}";
            }
            else
            {
                return true;
            }
        }
    }
}