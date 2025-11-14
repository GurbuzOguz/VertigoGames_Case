using UnityEngine;
using DG.Tweening;

public class WheelRotateController : MonoBehaviour
{
    [SerializeField] private Transform wheel;
    [SerializeField] private int sliceCount = 8;
    [SerializeField] private float spinDuration = 4f;

    [SerializeField] private float idleSpeed = 20f;
    [SerializeField] private float bounceAmount = 10f;
    [SerializeField] private float bounceDuration = 0.25f;

    private Tween idleTween;
    private Tween spinTween;

    private float sliceAngle;
    private float lastTickAngle;   // ✔ Artık slice algılamada bunu kullanıyoruz

    private void Start()
    {
        sliceAngle = 360f / sliceCount;
        lastTickAngle = GetWheelAngle();

        StartIdleSpin();
    }

    private void Update()
    {
        EmitTickEvents();  // ✔ Yeni slice geçiş algılaması
    }

    // ------------------------------------------------------
    //  ANGLE HELPERS
    // ------------------------------------------------------
    private float GetWheelAngle()
    {
        return (wheel.localEulerAngles.z + 360f) % 360f;
    }

    // ------------------------------------------------------
    //  NEW TICK SYSTEM (slice geçişini doğru hesaplayan)
    // ------------------------------------------------------
    private void EmitTickEvents()
    {
        float currentAngle = GetWheelAngle();

        float delta = Mathf.Abs(Mathf.DeltaAngle(lastTickAngle, currentAngle));

        // Slice sınırını geçtiysek tick üret
        if (delta >= sliceAngle)
        {
            WheelEvents.OnSliceTick?.Invoke();
            lastTickAngle = currentAngle; // ✔ bir sonraki slice hedefi
        }
    }

    private void OnEnable()
    {
        WheelEvents.OnSliceChosen += SpinToIndex;
    }

    private void OnDisable()
    {
        WheelEvents.OnSliceChosen -= SpinToIndex;
    }

    // ------------------------------------------------------
    //  IDLE ROTATION (spin başlamadan önceki yavaş dönüş)
    // ------------------------------------------------------
    private void StartIdleSpin()
    {
        idleTween = wheel
            .DORotate(new Vector3(0, 0, -360f), idleSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }

    private void StopIdleSpin()
    {
        idleTween?.Kill();
    }

    // ------------------------------------------------------
    //  MAIN SPIN
    // ------------------------------------------------------
    private void SpinToIndex(int index)
    {
        WheelEvents.OnSpinStarted?.Invoke();
        StopIdleSpin();

        float targetAngle = -(index * sliceAngle);
        float extraRotations = Random.Range(3, 6) * 360f;
        float finalAngle = extraRotations + targetAngle;

        // Next tick lifecycle reset → spin sırasında doğru tick hesaplanır
        lastTickAngle = GetWheelAngle();

        spinTween = wheel
            .DORotate(new Vector3(0, 0, finalAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                // Tiny bounce (casino effect)
                wheel
                    .DORotate(new Vector3(0, 0, finalAngle - bounceAmount), bounceDuration)
                    .SetEase(Ease.OutSine)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        WheelEvents.OnSpinCompleted?.Invoke();
                        //StartIdleSpin(); // İstersen aç
                         Debug.Log("STOPPED ANGLE = " + GetWheelAngle());
                    });
            });
    }
}
