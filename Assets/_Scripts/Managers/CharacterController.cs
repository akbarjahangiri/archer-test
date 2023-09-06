using UnityEngine;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    public float rotationDuration = 2f;
    public Vector3 startRotation = new Vector3(0f, 0f, 10f);
    public Vector3 endRotation = new Vector3(0f, 0f, -45f);

    [SerializeField] private GameObject body;

    private void Start()
    {
        // Call a method to start the rotation animation
        StartRotationAnimation();
    }

    private void StartRotationAnimation()
    {
        // Rotate the GameObject back and forth between start and end rotations in local space
        body.transform.DOLocalRotate(endRotation, rotationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // When the rotation is complete, reverse it to startRotation
                body.transform.DOLocalRotate(startRotation, rotationDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(StartRotationAnimation); // Loop by starting animation again
            });
    }
}