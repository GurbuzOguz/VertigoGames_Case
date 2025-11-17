using UnityEngine;
using DG.Tweening;

public class WheelRotateController : MonoBehaviour
{
    [SerializeField] private Transform wheel;
    [SerializeField] private float idleSpeed = 20f;
    [SerializeField] private float spinDuration = 4f;
    [SerializeField] private float bounceAmount = 10f;
    [SerializeField] private float bounceDuration = 0.25f;

    private Tween idleTween;
    private float lastAngle = 0f;

    private void Start()
    {
        StartIdleSpin();
    }

    private void OnEnable()
    {
        WheelEvents.OnRotateToAngle += SpinToAngle;
        WheelEvents.OnSpinRequest += OnSpinRequested;
    }

    private void OnDisable()
    {
        WheelEvents.OnRotateToAngle -= SpinToAngle;
        WheelEvents.OnSpinRequest -= OnSpinRequested;
    }

    private void OnSpinRequested()
    {
        WheelEvents.OnSpinStarted?.Invoke();
    }

    private void StopIdleSpin()
    {
        if (idleTween != null)
        {
            idleTween.Kill();
            idleTween = null;
        }
    }
    
    private void StartIdleSpin()
    {
        if (idleTween != null) return; 

        idleTween = wheel
            .DORotate(new Vector3(0, 0, wheel.localEulerAngles.z + 360f), idleSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .OnUpdate(() =>
            {
                SliceTickCheck();
            });
    }


    private void SpinToAngle(float finalAngle)
    {
        StopIdleSpin();
        lastAngle = wheel.localEulerAngles.z;

        wheel
            .DORotate(new Vector3(0, 0, finalAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .OnUpdate(SliceTickCheck)  
            .OnComplete(() =>
            {
                // wheel
                //     .DORotate(new Vector3(0, 0, finalAngle - bounceAmount), bounceDuration)
                //     .SetEase(Ease.OutSine)
                //     .SetLoops(2, LoopType.Yoyo)
                //     .OnComplete(NormalizeAndComplete);
                
                NormalizeAndComplete();
            });
    }

    private void SliceTickCheck()
    {
        float current = wheel.localEulerAngles.z;

        int prevIndex = Mathf.FloorToInt((lastAngle % 360f) / 45f);
        int currIndex = Mathf.FloorToInt((current % 360f) / 45f);

        if (prevIndex != currIndex)
        {
            WheelEvents.OnSliceTick?.Invoke();
        }

        lastAngle = current;
    }

    private void NormalizeAndComplete()
    {
        float normalized = wheel.localEulerAngles.z % 360f;
        wheel.localRotation = Quaternion.Euler(0, 0, normalized);

        WheelEvents.OnSpinCompleted?.Invoke();
        StartIdleSpin();
    }
}
