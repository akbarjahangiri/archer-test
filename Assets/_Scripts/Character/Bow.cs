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
        public Quaternion _idleRotation;


        private void Awake()
        {
            _arrowPool = new ObjectPool<Arrow>(CreateArrow, ActionOnGetArrow, ActionOnReleaseBullet,
                arrow => { Destroy(arrow.gameObject); }, false, magzineSize, magzineMaxSize);
            _availableArrows = magzineSize;
            GameManager.OnGameInit += HandleGameInit;
            GameManager.OnGameStageChanged += HandleStageChanged;
            _idleRotation = bowBody.transform.rotation;
        }

        private void OnDestroy()
        {
            GameManager.OnGameInit -= HandleGameInit;
            GameManager.OnGameStageChanged -= HandleStageChanged;
        }

        private void HandleGameInit()
        {
            GetArrow();
        }


        private void HandleStageChanged(GameState state)
        {
            switch (state)
            {
                case GameState.CharacterIdle:

                    break;
                case GameState.BowRotate:
                    HandleBowRotate();
                    break;
                case GameState.BowAim:
                    HandleBowAim();
                    break;
                case GameState.BowShoot:
                    HandleBowShoot();
                    break;
                case GameState.CheckArrow:
                    HandleCheckArrow();
                    break;
                case GameState.ReloadArrow:
                    HandleReloadArrow();
                    break;
                case GameState.Victory:
                    break;
                case GameState.Lose:
                    break;
            }
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


        // event handlers
        private void HandleBowRotate()
        {
            StartRotationAnimation();
        }

        private void HandleBowAim()
        {
            StopBowRotation();
            GameManager.Instance.ChangeGameState(GameState.BowChargeStart);
        }
        
        private void HandleBowShoot()
        {
            Debug.Log(" ForceAmount " + forceAmount);
            if (_loadedArrow.rigidbody2D != null)
            {
                _loadedArrow.rigidbody2D.gravityScale = 2.5f;
                _loadedArrow.rigidbody2D.AddForce(-_loadedArrow.transform.right * forceAmount, ForceMode2D.Impulse);
                _loadedArrow.isShooting = true;
            }

            _availableArrows -= 1;
            _loadedArrow = null;
            GameManager.Instance.ChangeGameState(GameState.CheckArrow);
        }

        private void HandleCheckArrow()
        {
            if (_availableArrows == 0)
            {
                StopBowRotation();
                GameManager.Instance.ChangeGameState(GameState.Lose);
                Debug.Log("Not arrow Left , Lost!");
            }
            else
            {
                GameManager.Instance.ChangeGameState(GameState.ReloadArrow);
            }
        }

        private void HandleReloadArrow()
        {
            GetArrow();
            bowBody.transform.DORotate(_idleRotation.eulerAngles, 0.3f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    GameManager.Instance.ChangeGameState(GameState.CharacterIdle);
                });
        }

        private void StartRotationAnimation()
        {
            _rotationTween = bowBody.transform.DOLocalRotate(endRotation, rotationDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    bowBody.transform.DOLocalRotate(startRotation, rotationDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(StartRotationAnimation); // Loop by starting animation again
                });
        }

        private void StopBowRotation()
        {
            if (_rotationTween != null && _rotationTween.IsActive())
            {
                _rotationTween.Pause();
                _rotationTween.Kill();
                
            }
        }
        

        private void GetArrow()
        {
            if (_arrowPool != null)
            {
                _loadedArrow = _arrowPool.Get();
            }
        }
    }
}