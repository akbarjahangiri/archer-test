using System;
using _Scripts.StateMachine;
using _Scripts.StateMachine.GameState;
using UnityEngine;

namespace Archer.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }


        private bool _canClick = false;
        private bool _canChangeState = true;
        private GameStateManager _gameStateManager;

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

            _gameStateManager = FindObjectOfType<GameStateManager>();
        }

        private void HandleGameInit()
        {
            _canClick = false;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_canClick)
            {
                _canClick = true;
                InputChange();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _canClick = false;
                Debug.Log(_canClick);
            }
        }

        private void InputChange()
        {
            if (_gameStateManager.currentState is IdleState && _canChangeState)
            {
                _gameStateManager.SwitchState(_gameStateManager.RotateState);
                _canChangeState = false;
            }

            if (_gameStateManager.currentState is RotateState && _canChangeState)
            {
                _gameStateManager.SwitchState(_gameStateManager.BowPowerChargeState);
                _canChangeState = false;
            }
            if (_gameStateManager.currentState is BowPowerChargeState && _canChangeState)
            {
                _gameStateManager.SwitchState(_gameStateManager.ShootState);
                _canChangeState = false;
            }

            _canChangeState = true;
        }
    }
}