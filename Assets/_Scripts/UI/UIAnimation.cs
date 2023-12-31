using System;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine.UI;

namespace Archer.UI
{
    public class UIAnimation : MonoBehaviour
    {
        [SerializeField] private float animationDuration = 1.0f;
        [SerializeField] private float delayBeforeStart = 0.0f;
        [SerializeField] private AnimationType animationType = AnimationType.FadeIn;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 _endPosition;
        [SerializeField] private Vector3 startScale = Vector3.one;
        [SerializeField] private Vector3 endScale = Vector3.one;
        [SerializeField] private Ease easeType = Ease.Linear;
        [SerializeField] private bool loop = false;
        [SerializeField] private bool playOnAwake = false;


        private RectTransform _rectTransform;
        private Image _image;

        public enum AnimationType
        {
            FadeIn,
            MoveFromOutside,
            Scale
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            if (animationType == AnimationType.FadeIn)
            {
                //_rectTransform.anchoredPosition = Vector2.zero;
                //_image.color = new Color(0, 0, 0, 0);
            }
            else if (animationType == AnimationType.MoveFromOutside)
            {
                _endPosition = _rectTransform.localPosition;

                _rectTransform.anchoredPosition = startPosition;
            }
            else if (animationType == AnimationType.Scale)
            {
                _rectTransform.localScale = startScale;
            }

            if (playOnAwake)
            {
                PlayAnimation();
            }
        }

        public void PlayAnimation()
        {
            if (animationType == AnimationType.FadeIn)
            {
                Debug.Log("Fade In");
                if (loop)
                {
                    _image.DOFade(1.0f, animationDuration).SetEase(easeType).SetDelay(delayBeforeStart).SetLoops(-1,LoopType.Yoyo);

                }
                else
                {
                    _image.DOFade(1.0f, animationDuration).SetEase(easeType).SetDelay(delayBeforeStart);

                }
            }
            else if (animationType == AnimationType.MoveFromOutside)
            {
                Debug.Log("MoveFromOutside");

                if (loop)
                {
                    _rectTransform
                        .DOAnchorPos(new Vector2(_endPosition.x, _endPosition.y),
                            animationDuration).SetEase(easeType).SetDelay(delayBeforeStart).SetLoops(-1,LoopType.Yoyo);
                }
                else
                {
                    _rectTransform
                        .DOAnchorPos(new Vector2(_endPosition.x, _endPosition.y),
                            animationDuration).SetEase(easeType).SetDelay(delayBeforeStart);
                }
            }
            else if (animationType == AnimationType.Scale)

            {
                if (loop)
                {
                    _rectTransform.DOScale(endScale, animationDuration).SetEase(easeType).SetDelay(delayBeforeStart).SetLoops(-1,LoopType.Yoyo);

                }
                else
                {
                    _rectTransform.DOScale(endScale, animationDuration).SetEase(easeType).SetDelay(delayBeforeStart);

                }
            }
        }
    }
}