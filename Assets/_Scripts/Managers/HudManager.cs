using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private TextMeshProUGUI minScore;
        [SerializeField] private TextMeshProUGUI playerScore;
        [SerializeField] private Button exitLevelButton;


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
        }

        
        private void OnDestroy()
        {
            GameManager.OnGameInit -= HandleGameInit;
            // GameManager.OnGameStageChanged -= HandleStageChanged;
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

        public void UpdateLevelInfo(string levelNumber,string minScore)
        {
            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            this.levelNumber.text = levelNumber;
            this.minScore.text = minScore;
        }
  
    }
}