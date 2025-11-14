using UnityEngine;
using DG.Tweening;

public class IndicatorClickController : MonoBehaviour
{
    private void OnEnable()
    {
        WheelEvents.OnSliceTick += PlayTickAnimation;
    }

    private void OnDisable()
    {
        WheelEvents.OnSliceTick -= PlayTickAnimation;
    }

    private void PlayTickAnimation()
    {
        transform.DOKill();
        transform.localRotation = Quaternion.identity;

        transform
            .DOLocalRotate(new Vector3(0, 0, -15f), 0.05f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform
                    .DOLocalRotate(Vector3.zero, 0.08f)
                    .SetEase(Ease.OutCubic);
            });
    }
}