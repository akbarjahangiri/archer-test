using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.Managers
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private Slider powerSlider;
        [SerializeField] private float powerChargeDuration;
        [SerializeField] private float minValue = 0f;
        [SerializeField] private float maxValue = 1f;
        [SerializeField] private TextMeshProUGUI levelNumber;
        [SerializeField] private GameObject nextLevelPopup;
        [SerializeField] private TextMeshProUGUI nextLevelNumber;
        [SerializeField] private TextMeshProUGUI minScore;
        [SerializeField] private TextMeshProUGUI playerScore;
        [SerializeField] private Button exitLevelButton;
        [SerializeField] private Button loseExitLevelButton;
        [SerializeField] private RectTransform loadingNextLevelImage;
        [SerializeField] private RectTransform victoryPanel;
        [SerializeField] private RectTransform losePanel;
        [SerializeField] private TextMeshProUGUI arrowCount;
        [SerializeField] private TextMeshProUGUI victoryScore;
        [SerializeField] private GameObject victoryPlayerName;
        [SerializeField] private Button saveVictoryResultButton;
        private Sequence _sequence;


        private Tween _powerChargeTween;

        private Bow bow;

        private void Awake()
        {
            GameObject bowGameObject = GameObject.FindWithTag("Bow");
            if (bowGameObject != null)
            {
                bow = bowGameObject.GetComponent<Bow>();
            }
            else
            {
                Debug.LogError("Bow GameObject not found.");
            }

            exitLevelButton.onClick.AddListener(GameManager.Instance.ExitLevel);
            loseExitLevelButton.onClick.AddListener(GameManager.Instance.ExitLevel);
            saveVictoryResultButton.onClick.AddListener(SaveVictoryScore);
        }


        private void OnDestroy()
        {
            exitLevelButton.onClick.RemoveListener(GameManager.Instance.ExitLevel);
            loseExitLevelButton.onClick.RemoveListener(GameManager.Instance.ExitLevel);
            saveVictoryResultButton.onClick.RemoveListener(SaveVictoryScore);
        }

        private void HandleGameInit()
        {
            // init 
        }

        public void HandleIdleState()
        {
            powerSlider.value = 0;
        }


        public void HandleBowPowerChargeStart()
        {
            _powerChargeTween = powerSlider.DOValue(maxValue, powerChargeDuration).SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void HandleBowPowerChargeEnd()
        {
            if (_powerChargeTween != null)
            {
                _powerChargeTween.Kill();
                bow.forceAmount = powerSlider.value;
            }
        }

        public void UpdateScore(string score)
        {
            playerScore.text = score;
        }

        public void UpdateLevelInfo(LevelData currentLevelData, PlayerProgress playerProgress)
        {
            levelNumber.text = playerProgress.currentLevel.ToString();
            minScore.text = currentLevelData.requiredScore.ToString();
            playerScore.text = playerProgress.score.ToString();
        }

        public void StartloadingNextLevelEffect()
        {
            if (loadingNextLevelImage != null)
            {
                loadingNextLevelImage.gameObject.SetActive(true);

                _sequence = DOTween.Sequence();
                _sequence.Append(
                    loadingNextLevelImage.transform.DORotate(new Vector3(0, 0, 360), 4, RotateMode.FastBeyond360));
                _sequence.Join(loadingNextLevelImage.transform.DOScale(new Vector3(11, 11, 11), 4));
                _sequence.SetEase(Ease.InOutBack);
                _sequence.Play();
                _sequence.OnComplete(() =>
                {
                    nextLevelPopup.SetActive(true);
                    Debug.Log("Curennt level nuuuuuuumber  " + GameManager.Instance.currentLevelDataNumber);
                    nextLevelNumber.text = (GameManager.Instance.GetCurrentLevelNumber() + 1).ToString();
                    _sequence = DOTween.Sequence();
                    _sequence.Append(
                        loadingNextLevelImage.transform.DORotate(Vector3.zero, 3,
                            RotateMode.FastBeyond360)); // Reverse rotation
                    _sequence.Join(loadingNextLevelImage.transform.DOScale(Vector3.one, 3)); // Reverse scaling
                    _sequence.SetEase(Ease.OutElastic);
                });
            }
        }

        public bool CheckloadingNextLevelEffectStatus()
        {
            if (_sequence != null && !_sequence.active)
            {
                _sequence = null;
                return true;
            }

            return false;
        }

        public void ResetloadingNextLevelEffectStatus()
        {
            if (_sequence != null && !_sequence.active)
            {
                nextLevelPopup.SetActive(false);
                loadingNextLevelImage.gameObject.SetActive(false);
                loadingNextLevelImage.transform.localScale = new Vector3(1, 1, 1);
                loadingNextLevelImage.transform.rotation = new Quaternion(0, 0, 0, 0);
                nextLevelNumber.gameObject.SetActive(false);
            }
        }

        public void ShowVictoryPanel()
        {
            victoryPanel.gameObject.SetActive(true);
        }

        private void SaveVictoryScore()
        {
            GameManager.Instance.AddPlayerScore(victoryPlayerName.GetComponent<TMP_InputField>().text);
            GameManager.Instance.LoadMainMenu();
        }

        public void RemoveArrow(string remainArrows)
        {
            arrowCount.text = "x" + remainArrows;
        }

        public void HandleLose()
        {
            losePanel.gameObject.SetActive(true);
        }
    }
}