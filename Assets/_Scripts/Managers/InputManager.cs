using System;
using UnityEngine;

namespace Archer.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        public event Action OnRotate;
        public event Action OnCharge;
        public event Action OnShoot;

        private bool isRotating = false;
        private bool isCharging = false;
        private bool isShooting = false;
        private bool hasClicked = false;


        private enum InputState
        {
            Idle,
            Rotating,
            Charging,
            Shooting,
        }

        private InputState currentState = InputState.Idle;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !hasClicked)
            {
                switch (currentState)
                {
                    case InputState.Idle:
                        HandleIdleState();
                        break;

                    case InputState.Rotating:
                        HandleRotatingState();
                        break;

                    case InputState.Charging:
                        HandleChargingState();
                        break;

                    case InputState.Shooting:
                        HandleShootingState();
                        break;
                }

                hasClicked = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                hasClicked = false;
            }
        }

        private void HandleIdleState()
        {
            Debug.Log("click start Rotation");
            currentState = InputState.Rotating;
            OnRotate?.Invoke();
        }

        private void HandleRotatingState()
        {
            if (!isCharging)
            {
                Debug.Log("click start Charging");
                currentState = InputState.Charging;
                OnCharge?.Invoke();
            }

            /*else
            {
                currentState = InputState.Idle;
            }*/
        }

        private void HandleChargingState()
        {
            if (!isShooting)
            {
                Debug.Log("click start Shooting");
                currentState = InputState.Shooting;
                OnShoot?.Invoke();
            }

            /*else
            {
                currentState = InputState.Idle;
            }*/
        }

        private void HandleShootingState()
        {
            Debug.Log("Shooting complete");
            currentState = InputState.Idle;
        }
    }
}