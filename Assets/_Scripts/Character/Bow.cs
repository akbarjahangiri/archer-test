using System;
using Archer.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Archer.Managers
{
    public class Bow : MonoBehaviour
    {
        private IObjectPool<Arrow> _arrowPool;
        private Arrow _loadedArrow;
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private int magzineSize = 5;
        private int _availableArrows;
        [SerializeField] private int magzineMaxSize = 50;
        public float forceAmount = 1;
        [SerializeField] private GameObject bowBody;
        private Tween _rotationTween;

        public float rotationDuration = 2f;
        public Vector3 startRotation = new Vector3(0f, 0f, 10f);
        public Vector3 endRotation = new Vector3(0f, 0f, -45f);

        public event Action OnRotate;
        public event Action OnPauseRotate;
        public event Action OnCharge;

        public event Action OnShoot;
        public event Action OnCheckArrow;
        public event Action OnReloadArrow;

        private void Awake()
        {
            _arrowPool = new ObjectPool<Arrow>(CreateArrow, ActionOnGetArrow, ActionOnReleaseBullet,
                arrow => { Destroy(arrow.gameObject); }, false, magzineSize, magzineMaxSize);
            _availableArrows = magzineSize;
            GameManager.OnGameInit += HandleGameInit;
        }

        private void OnDestroy()
        {
            GameManager.OnGameInit -= HandleGameInit;
        }

        private void HandleGameInit()
        {
            GetArrow();
        }

        private Arrow CreateArrow()
        {
            Arrow arrow = Instantiate(this.arrowPrefab);
            arrow.transform.SetParent(transform, false);
            arrow.rigidbody2D.gravityScale = 0;
            return arrow;
        }

        private void ActionOnGetArrow(Arrow arrow)
        {
            Debug.Log("Get Arrow from pool");
            _loadedArrow = arrow;
            _loadedArrow.gameObject.SetActive(true);
        }

        private void ActionOnReleaseBullet(Arrow obj)
        {
            arrowPrefab.gameObject.SetActive(false);
        }

        private void GetArrow()
        {
            if (_arrowPool != null)
            {
                Debug.Log("Get Arrow");
                _loadedArrow = _arrowPool.Get();
            }
        }

        // event handlers
        public void InvokeRotateEvent()
        {
            StartRotationAnimation();
            OnRotate?.Invoke();
        }

        public void InvokeChargeEvent()
        {
            PauseBowRotation();
            GameManager.Instance.CurrentGameState = GameState.BowShoot;
            OnCharge?.Invoke();
        }

        public void InvokeShootEvent()
        {
            Debug.Log(" ForceAmount " + forceAmount);

            if (_loadedArrow.rigidbody2D != null)
            {
                OnShoot?.Invoke();

                _loadedArrow.rigidbody2D.gravityScale = 2.5f;
                _loadedArrow.rigidbody2D.AddForce(-_loadedArrow.transform.right * forceAmount, ForceMode2D.Impulse);
                _loadedArrow.isShooting = true;
            }

            _availableArrows -= 1;
            _loadedArrow = null;
            Debug.Log("Change state to CheckArrow");
            //GameManager.Instance.CurrentGameState = GameState.CheckArrow;

        }

        public void InvokeCheckArrowEvent()
        {
            Debug.Log("Checking arrow count...  Arrow Count = " + _availableArrows);
            if (_availableArrows == 0)
            {
                GameManager.Instance.ChangeGameState(GameState.Lose);
                Debug.Log("Not arrow Left , Lost!");
            }
            else
            {
                Debug.Log("Change state to ReloadArrow");
                GameManager.Instance.ChangeGameState(GameState.ReloadArrow);
            }

            OnCheckArrow?.Invoke();
        }

        public void InvokeReloadArrowEvent()
        {
            Debug.Log("ReloadArrow......!");
            GetArrow();
            GameManager.Instance.CurrentGameState = GameState.BowRotate;
            OnReloadArrow?.Invoke();
        }

        private void StartRotationAnimation()
        {
            // Rotate the GameObject back and forth between start and end rotations in local space
            _rotationTween = bowBody.transform.DOLocalRotate(endRotation, rotationDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    // When the rotation is complete, reverse it to startRotation
                    bowBody.transform.DOLocalRotate(startRotation, rotationDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(StartRotationAnimation); // Loop by starting animation again
                });
            GameManager.Instance.ChangeGameState(GameState.BowCharge);
        }

        public void PauseBowRotation()
        {
            if (_rotationTween != null && _rotationTween.IsActive())
            {
                Debug.Log("Pause Rotation");

                bowBody.transform.DOPause();
            }
        }

        public void ResumeRotation()
        {
            // Resume the paused rotation animation
            if (_rotationTween != null && !_rotationTween.IsActive())
            {
                _rotationTween.Play();
            }
        }
    }
}