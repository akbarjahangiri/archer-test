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
            if (Input.GetMouseButton(0))
            {
                if (!isRotating)
                {
                    Debug.Log("click");

                    isRotating = true;
                    OnRotate?.Invoke();
                }
                else
                {
                    if (!isCharging)
                    {
                        isCharging = true;
                        isRotating = false;
                        OnCharge?.Invoke();
                    }
                }
            }
            else if (Input.GetMouseButton(1) && isCharging)
            {
                OnShoot?.Invoke();
                isCharging = false;
            }
        }
    }
}