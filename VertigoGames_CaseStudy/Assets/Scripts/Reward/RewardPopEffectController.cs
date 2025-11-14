using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class RewardPopEffectController : MonoBehaviour
{
    public static RewardPopEffectController Instance;

    [Header("Settings")]
    [SerializeField] private int popCount = 5;
    [SerializeField] private float popRadius = 100f;
    [SerializeField] private float popDuration = 0.35f;
    [SerializeField] private float travelDuration = 0.5f;

    [Header("References")]
    [SerializeField] private RectTransform popSpawnPoint;     // Çarkın yakınında bir UI nokta
    [SerializeField] private Canvas mainCanvas;               // Ana canvas (UI space conversion için)
    [SerializeField] private Image popIconPrefab;             // Küçük uçan icon prefabı

    private void Awake()
    {
        Instance = this;
    }

    public void PlayPopEffect(WheelSliceData data, Transform targetIconTransform, Action onComplete)
    {
        StartCoroutine(PopRoutine(data, targetIconTransform, onComplete));
    }

    private System.Collections.IEnumerator PopRoutine(WheelSliceData data, Transform target, Action onComplete)
    {
        List<Image> icons = new List<Image>();

        // 1) Random Outward pop
        for (int i = 0; i < popCount; i++)
        {
            var img = Instantiate(popIconPrefab, popSpawnPoint.parent);
            img.sprite = data.iconSprite;
            img.transform.position = popSpawnPoint.position;
            img.color = new Color(1, 1, 1, 0);

            icons.Add(img);

            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle.normalized * popRadius;
            Vector3 targetPos = popSpawnPoint.position + (Vector3)randomOffset;

            img.DOFade(1f, 0.1f);
            img.transform
                .DOMove(targetPos, popDuration)
                .SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.05f); // küçük gecikme
        }

        // 2) Hepsi reward item'a doğru uçsun
        Vector3 endPos = target.position;

        foreach (var icon in icons)
        {
            icon.transform
                .DOMove(endPos, travelDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    icon.transform.DOScale(0f, 0.15f).OnComplete(() =>
                    {
                        Destroy(icon.gameObject);
                    });
                });
        }

        // 3) UI icon scale bounce
        target.DOScale(1.25f, 0.15f)
              .SetLoops(2, LoopType.Yoyo)
              .SetEase(Ease.OutQuad);

        // 4) Pop effect tamamlandı
        yield return new WaitForSeconds(travelDuration);
        onComplete?.Invoke();
    }
}