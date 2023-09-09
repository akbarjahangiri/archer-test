using System;
using UnityEngine;

namespace Archer.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }


        private bool hasClicked = false;
        private Bow bow;

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

            GameObject
                bowGameObject =
                    GameObject.FindWithTag("Bow");
            if (bowGameObject != null)
            {
                bow = bowGameObject.GetComponent<Bow>();
                if (bow != null)
                {
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

        private void HandleGameInit()
        {
            hasClicked = false;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !hasClicked)
            {
                Debug.Log("clicked  " + GameManager.Instance.CurrentGameState);
                hasClicked = true;
                HandleGameStateBasedOnInput();
            }

            if (Input.GetMouseButtonUp(0))
            {
                hasClicked = false;
            }
        }

        private void HandleGameStateBasedOnInput()
        {
            if (GameManager.Instance.CurrentGameState == GameState.CharacterIdle)
            {
                Debug.Log("input manager CharacterIdle....");
                bow.InvokeRotateEvent();
            }
            
            else if (GameManager.Instance.CurrentGameState == GameState.BowCharge)
            {
               // GameManager.Instance.ChangeGameState(GameState.BowShoot);
                bow.InvokeChargeEvent();
            }
            else if (GameManager.Instance.CurrentGameState == GameState.BowShoot)
            {
                // GameManager.Instance.ChangeGameState(GameState.BowShoot);
                bow.InvokeShootEvent();
            }
        }
    }
}