using UnityEngine;
using DG.Tweening;

public class RotateLoop : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 0, -360f);
    [SerializeField] private float duration = 3f;

    [Header("Loop Settings")]
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private LoopType loopType = LoopType.Restart;

    private Tween rotateTween;

    private void OnEnable()
    {
        StartRotation();
    }

    private void OnDisable()
    {
        rotateTween?.Kill();
    }

    private void StartRotation()
    {
        rotateTween?.Kill();

        rotateTween = transform
            .DORotate(rotationAxis, duration, RotateMode.FastBeyond360)
            .SetEase(easeType)
            .SetLoops(-1, loopType)
            .SetUpdate(true);
    }
}