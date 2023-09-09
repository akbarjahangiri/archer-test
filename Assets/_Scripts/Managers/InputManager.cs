using System;
using UnityEngine;

namespace Archer.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }


        private bool _canClick = false;

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

            GameManager.OnGameInit += HandleGameInit;
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
            switch (GameManager.Instance.currentGameState)
            {
                case GameState.CharacterIdle:
                    GameManager.Instance.ChangeGameState(GameState.BowRotate);
                    break;
                case GameState.BowRotate:
                    GameManager.Instance.ChangeGameState(GameState.BowAim);
                    break;
                case GameState.BowChargeStart:
                    GameManager.Instance.ChangeGameState(GameState.BowChargeEnd);
                    break;
                case GameState.Lose:
                    _canClick = false;
                    break;
            }
        }
    }
}