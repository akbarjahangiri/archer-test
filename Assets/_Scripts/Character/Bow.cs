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
        [SerializeField] private int quiverSize = 5;
        private int _availableQuiverArrows;
        [SerializeField] private int quiverMaxSize = 50;
        public float forceAmount = 100;
        [SerializeField] private GameObject bowBody;
        private Tween _rotationTween;

        public float rotationDuration = 2f;
        public Vector3 endRotation = new Vector3(0f, 0f, -45f);
        public Quaternion _idleRotation;


        private void Awake()
        {
            _arrowPool = new ObjectPool<Arrow>(CreateArrow, ActionOnGetArrow, ActionOnReleaseBullet,
                arrow => { Destroy(arrow.gameObject); }, false, quiverSize, quiverMaxSize);
            _availableQuiverArrows = quiverSize;
            _idleRotation = bowBody.transform.rotation;
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


        // event handlers
        public void HandleShootState()
        {
            Debug.Log(" ForceAmount " + forceAmount);
            if (_loadedArrow.rigidbody2D != null)
            {
                _loadedArrow.rigidbody2D.gravityScale = 2.5f;
                _loadedArrow.sparkParticle.gameObject.SetActive(true);
                _loadedArrow.rigidbody2D.AddForce(-_loadedArrow.transform.right * forceAmount, ForceMode2D.Impulse);
                _loadedArrow.isShooting = true;
            }

            _availableQuiverArrows -= 1;
            _loadedArrow = null;
        }

        public bool HandleCheckArrow()
        {
            if (_availableQuiverArrows == 0)
            {
                return false;
            }

            return true;
        }

        public void HandleReloadArrow()
        {
            GetArrow();
            bowBody.transform.DORotate(_idleRotation.eulerAngles, 0.3f)
                .SetEase(Ease.Linear);
        }

        public void StartRotationAnimation()
        {
            _rotationTween = bowBody.transform.DOLocalRotate(endRotation, rotationDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void StopBowRotation()
        {
            if (_rotationTween != null && _rotationTween.IsActive())
            {
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