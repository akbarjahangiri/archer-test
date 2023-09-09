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

        private Bow bow;

        private void Awake()
        {
            GameObject bowGameObject = GameObject.FindWithTag("Bow");
            if (bowGameObject != null)
            {
                bow = bowGameObject.GetComponent<Bow>();
                if (bow != null)
                {
                    bow.OnCharge += HandleBowCharge;
                    bow.OnCheckArrow += HandleCheckArrow;
                    bow.OnReloadArrow += HandleReloadArrow;
                    bow.OnShoot += HandeShootArrow;
                }
                else
                {
                    Debug.LogError("Bow script not found on the Bow GameObject.");
                }
            }
            else
            {
                Debug.LogError("Bow GameObject not found.");
            }

            GameManager.OnGameInit += HandleGameInit;
        }


        private void OnDestroy()
        {
            bow.OnCharge -= HandleBowCharge;
            bow.OnCheckArrow -= HandleCheckArrow;
            bow.OnReloadArrow -= HandleReloadArrow;
            bow.OnShoot -= HandeShootArrow;
            GameManager.OnGameInit -= HandleGameInit;
        }

        private void HandleGameInit()
        {
            // init 
        }

        private void HandleBowCharge()
        {
            _powerChargeTween = powerSlider.DOValue(maxValue, powerChargeDuration).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    powerSlider.DOValue(minValue, powerChargeDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(HandleBowCharge);
                });
        }

        public void HandeShootArrow()
        {
            if (_powerChargeTween != null)
            {
                Debug.Log("HUD ForceAmount " + powerSlider.value);
                bow.forceAmount = powerSlider.value;
                Debug.Log("Pause Charge ui");
                _powerChargeTween.Pause();
            }
        }

        // Event handling methods for Bow events
        private void HandleBowRotate()
        {
            // Handle Bow rotation event in the HUD
        }

        private void HandleBowShoot()
        {
            // Handle Bow shooting event in the HUD
        }

        private void HandleCheckArrow()
        {
            // Handle Bow check arrow event in the HUD
        }

        private void HandleReloadArrow()
        {
            // Handle Bow reload arrow event in the HUD
        }
    }
}