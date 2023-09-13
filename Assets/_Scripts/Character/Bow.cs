using System;
using System.Collections.Generic;
using Archer.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Archer.Managers
{
    public class Bow : MonoBehaviour
    {
        private IObjectPool<Arrow> _arrowPool;
        private Arrow _loadedArrow;
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private int quiverSize = 5;
         public int availableQuiverArrows;
        [SerializeField] private int quiverMaxSize = 50;
        public float forceAmount = 100;
        [SerializeField] private GameObject bowBody;
        private Tween _rotationTweenBot;
        private Sequence _rotationSequence;

        public float rotationDuration = 1f;
        public Vector3 endRotation = new Vector3(0f, 0f, -45f);
        public Vector3 startRotation = new Vector3(0f, 0f, 10f);
        public Quaternion _idleRotation;
        [SerializeField] private List<GameObject> arrows;


        private void Awake()
        {
            _arrowPool = new ObjectPool<Arrow>(CreateArrow, ActionOnGetArrow, ActionOnReleaseBullet,
                arrow => { Destroy(arrow.gameObject); }, false, quiverSize, quiverMaxSize);
            availableQuiverArrows = quiverSize;
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

            availableQuiverArrows -= 1;
            var lastGameObject = arrows[arrows.Count - 1];
            lastGameObject.SetActive(false);
            arrows.RemoveAt(arrows.Count - 1);
            _loadedArrow = null;
        }

        public bool HandleCheckArrow()
        {
            if (availableQuiverArrows == 0)
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
            _rotationSequence = DOTween.Sequence();

            _rotationSequence.Append(bowBody.transform.DOLocalRotate(endRotation, rotationDuration)
                .SetEase(Ease.Linear));

            _rotationSequence.Append(bowBody.transform.DOLocalRotate(startRotation, rotationDuration)
                .SetEase(Ease.Linear));

            _rotationSequence.SetLoops(-1, LoopType.Yoyo);

            _rotationSequence.Play();
        }

        public void StopBowRotation()
        {
            _rotationSequence.Kill();

            /*if (_rotationTweenUp != null && _rotationTweenUp.IsActive())
            {
                Debug.Log("killing rotation");
                _rotationTweenUp.Kill();
            }*/
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