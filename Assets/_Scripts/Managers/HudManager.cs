using DG.Tweening;
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

        private Tween _powerChargeTween;

        private Bow bow;

        private void Awake()
        {
            GameManager.OnGameStageChanged += HandleStageChanged;
            GameManager.OnGameInit += HandleGameInit;

            GameObject bowGameObject = GameObject.FindWithTag("Bow");
            if (bowGameObject != null)
            {
                bow = bowGameObject.GetComponent<Bow>();
            }
            else
            {
                Debug.LogError("Bow GameObject not found.");
            }
        }

        private void HandleStageChanged(GameState state)
        {
            switch (state)
            {
                case GameState.CharacterIdle:
                    HandleICharacterIdle();
                    break;
                case GameState.BowChargeStart:
                    HandleBowChargeStart();
                    break;
                case GameState.BowChargeEnd:
                    HandleBowChargeEnd();
                    break;
                case GameState.BowShoot:
                    HandeShootArrow();
                    break;
                case GameState.CheckArrow:
                    break;
                case GameState.ReloadArrow:
                    break;
                case GameState.Victory:
                    break;
                case GameState.Lose:
                    HandeLose();
                    break;
            }
        }


        private void OnDestroy()
        {
            GameManager.OnGameInit -= HandleGameInit;
            GameManager.OnGameStageChanged -= HandleStageChanged;
        }

        private void HandleGameInit()
        {
            // init 
        }

        private void HandleICharacterIdle()
        {
            powerSlider.value = 0;
        }


        private void HandleBowChargeStart()
        {
            _powerChargeTween = powerSlider.DOValue(maxValue, powerChargeDuration).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    powerSlider.DOValue(minValue, powerChargeDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(HandleBowChargeStart);
                });
        }

        private void HandleBowChargeEnd()
        {
            if (_powerChargeTween != null)
            {
                _powerChargeTween.Pause();
                _powerChargeTween.Kill();
                bow.forceAmount = powerSlider.value;
                GameManager.Instance.ChangeGameState(GameState.BowShoot);
            }
        }

        public void HandeShootArrow()
        {
         
        }

        private void HandeLose()
        {
            if (_powerChargeTween != null)
            {
                _powerChargeTween.Pause();
                _powerChargeTween.Kill();
            }
        }
    }
}