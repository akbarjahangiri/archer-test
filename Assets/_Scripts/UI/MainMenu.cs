using Archer.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class MainMenu : UIBase
    {
        [SerializeField] private GameObject scoreboardUI;
        [SerializeField] private Button newGameButton;

        [SerializeField] private Button loadGameButton;

        [SerializeField] private UIAnimation[] _uiAnimations;
        [SerializeField] private RectTransform scorePrefab;
        [SerializeField] private RectTransform scoresParent;

        void Start()
        {
            newGameButton.onClick.AddListener(OnNewGameClick);
            loadGameButton.onClick.AddListener(OnLoadGameClick);
            InitMainMenu();
        }

        void OnDestroy()
        {
            newGameButton.onClick.RemoveListener(OnNewGameClick);
            loadGameButton.onClick.RemoveListener(OnLoadGameClick);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
            // Play animations for each button
            foreach (var buttonAnimation in _uiAnimations)
            {
                buttonAnimation.PlayAnimation();
            }
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnNewGameClick()
        {
            GameManager.Instance.StartNewGame();
        }

        public void OnLoadGameClick()
        {
            GameManager.Instance.LoadGame();

            Debug.Log("OnLoadGameClick");
        }

        private void InitMainMenu()
        {
            if (GameManager.Instance.CheckPlayerSave())
            {
                loadGameButton.interactable = false;
            }
            else
            {
                loadGameButton.interactable = true;
            }

            if (GameManager.Instance.LoadPlayerScores() != null)
            {
                LoadScoreboard(GameManager.Instance.LoadPlayerScores());
            }
        }

        private void LoadScoreboard(PlayerScores playerscores)
        {
            /*foreach (RectTransform child in scoresParent)
            {
                Destroy(child.gameObject);
            }*/

            foreach (var playerScoreEntry in playerscores.scores)
            {
                RectTransform scoreItem = Instantiate(scorePrefab, scoresParent);
                scoreItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text =
                    $"{playerScoreEntry.name}";
                scoreItem.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text =
                    $"{playerScoreEntry.score}";
            }
        }
    }
}