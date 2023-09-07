using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;

namespace Archer.Managers
{
    public class CharacterController : MonoBehaviour
    {
        public float rotationDuration = 2f;
        public Vector3 startRotation = new Vector3(0f, 0f, 10f);
        public Vector3 endRotation = new Vector3(0f, 0f, -45f);

        [SerializeField] private GameObject body;
        private IObjectPool<Arrow> _arrowPool;
        private Tween _rotationTween;

        private void Start()
        {
            // Call a method to start the rotation animation
            InputManager.Instance.OnRotate += StartRotationAnimation;
            InputManager.Instance.OnCharge += StartCharging;
        }

        private void StartCharging()
        {
            PauseRotation();
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnRotate -= StartRotationAnimation;
        }

        private void StartRotationAnimation()
        {
            // Rotate the GameObject back and forth between start and end rotations in local space
            _rotationTween = body.transform.DOLocalRotate(endRotation, rotationDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    // When the rotation is complete, reverse it to startRotation
                    body.transform.DOLocalRotate(startRotation, rotationDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(StartRotationAnimation); // Loop by starting animation again
                });
        }

        public void PauseRotation()
        {
            if (_rotationTween != null && _rotationTween.IsActive())
            {
                Debug.Log("Pause Rotation");

                body.transform.DOPause();
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