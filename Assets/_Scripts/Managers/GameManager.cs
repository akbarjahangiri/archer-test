using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Archer.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public LevelData[] levelDataArray;
        private LevelData currentLevelData;
        private PlayerProgress playerProgress;
        public int playerLevelScore;


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

        private void LoadPlayerProgress()
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
                SavePlayerProgress();
            }
        }

        public void SavePlayerProgress()
        {
            string json = JsonUtility.ToJson(playerProgress);
            string filePath = Application.persistentDataPath + "/playerProgress.json";
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

        public void LoadPlayerProgressFromJSON()
        {
            // Load player progress from a JSON file
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


        public void StartNewGame()
        {
            currentLevelData = levelDataArray[0];
            playerProgress = new PlayerProgress();
            playerProgress.currentLevel = 1;
            Debug.Log(playerProgress);
            LoadGameplay();
        }

        public void LoadGame()
        {
            LoadPlayerProgressFromJSON();
        }


        public bool CheckPlayerProgress()
        {
            if (playerLevelScore >= currentLevelData.requiredScore)
            {
                return true;
            }

            return false;
        }

        // Check if this is the level of the game
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
            // Get the next level number
            int nextLevelNumber = currentLevelData.levelNumber + 1;

            // Check if the next level number is valid
            if (nextLevelNumber >= 1 && nextLevelNumber <= levelDataArray.Length)
            {
                // Set the current level data to the next level data
                currentLevelData = levelDataArray[nextLevelNumber - 1];

                // Load the next level scene
                LoadGameplay();
            }
            else
            {
                Debug.LogError("Invalid next level number.");
            }
        }
    }
}