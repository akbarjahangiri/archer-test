using System;
using System.Collections;
using System.Collections.Generic;
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

        // Start is called before the first frame update
        void Start()
        {
            InputManager.Instance.OnCharge += StartChargeLoop;
            InputManager.Instance.OnShoot += PauseChargeLoop;
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnCharge -= StartChargeLoop;
            InputManager.Instance.OnShoot -= PauseChargeLoop;        }

        private void StartChargeLoop()
        {
            _powerChargeTween = powerSlider.DOValue(maxValue, powerChargeDuration).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    powerSlider.DOValue(minValue, powerChargeDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(StartChargeLoop);
                });
        }

        public void PauseChargeLoop()
        {
            if (_powerChargeTween != null)
            {
                _powerChargeTween.Pause();
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}