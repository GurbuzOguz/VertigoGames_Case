using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WheelRotateController : MonoBehaviour
{
    [SerializeField] private Transform wheel;
    [SerializeField] private float idleSpeed = 20f;
    [SerializeField] private float spinDuration = 4f;

    private Tween idleTween;
    private Tween spinTween;

    private float lastAngle = 0f;
    private bool skipEnabled = false;

    private void Start()
    {
        StartIdleSpin();
    }

    private void Update()
    {
        if (skipEnabled && spinTween != null && spinTween.IsActive())
        {
            if (!EventSystem.current.IsPointerOverGameObject() &&
                Input.GetMouseButtonDown(0))
            {
                spinTween.Complete();
            }
        }
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
        idleTween?.Kill();
        idleTween = null;
    }

    private void StartIdleSpin()
    {
        if (idleTween != null) return;

        idleTween = wheel
            .DORotate(new Vector3(0, 0, wheel.localEulerAngles.z + 360f), idleSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .OnUpdate(SliceTickCheck);
    }

    private void SpinToAngle(float finalAngle)
    {
        StopIdleSpin();
        lastAngle = wheel.localEulerAngles.z;

        skipEnabled = false;

        spinTween = wheel
            .DORotate(new Vector3(0, 0, finalAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .OnUpdate(SliceTickCheck)
            .OnStart(() => StartCoroutine(EnableSkipNextFrame()))
            .OnComplete(() =>
            {
                skipEnabled = false;
                spinTween = null;
                NormalizeAndComplete();
            });
    }

    private System.Collections.IEnumerator EnableSkipNextFrame()
    {
        yield return null; 
        skipEnabled = true; 
    }

    private void SliceTickCheck()
    {
        float current = wheel.localEulerAngles.z;

        int prevIndex = Mathf.FloorToInt((lastAngle % 360f) / 45f);
        int currIndex = Mathf.FloorToInt((current % 360f) / 45f);

        if (prevIndex != currIndex)
            WheelEvents.OnSliceTick?.Invoke();

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
