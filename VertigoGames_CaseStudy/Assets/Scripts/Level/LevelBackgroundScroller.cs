using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class LevelBackgroundScroller : MonoBehaviour
{
    [Header("BG RectTransforms")]
    [SerializeField] private RectTransform bgCurrent;
    [SerializeField] private RectTransform bgNext;

    [Header("BG Images")]
    [SerializeField] private Image currentImage;
    [SerializeField] private Image nextImage;

    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite goldSprite;

    [Header("Settings")]
    [SerializeField] private float spacing = 400f;
    [SerializeField] private float scrollAmount = 300f;
    [SerializeField] private float scrollDuration = 0.35f;

    private bool initialized = false;

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return null;
        ResetPositions();
        initialized = true;

        currentImage.sprite = GetSpriteForLevel(1);
        nextImage.sprite = GetSpriteForLevel(2);
    }

    private void ResetPositions()
    {
        bgCurrent.anchoredPosition = Vector2.zero;
        bgNext.anchoredPosition = new Vector2(spacing, 0f);
    }

    private void OnEnable()
    {
        WheelEvents.OnLevelNumberChanged += LevelChanged;
    }

    private void OnDisable()
    {
        WheelEvents.OnLevelNumberChanged -= LevelChanged;
    }

    private void LevelChanged(int level)
    {
        if (!initialized) return;

        nextImage.sprite = GetSpriteForLevel(level);
        Scroll();
    }

    private Sprite GetSpriteForLevel(int level)
    {
        if (level == 1 || level % 5 == 0)
            return greenSprite;

        if (level % 30 == 0)
            return goldSprite;

        return normalSprite;
    }

    private void Scroll()
    {
        bgCurrent.DOKill();
        bgNext.DOKill();

        float target = -scrollAmount;

        bgCurrent.DOAnchorPosX(bgCurrent.anchoredPosition.x - scrollAmount, scrollDuration)
                 .SetEase(Ease.OutCubic);

        bgNext.DOAnchorPosX(bgNext.anchoredPosition.x - scrollAmount, scrollDuration)
               .SetEase(Ease.OutCubic)
               .OnComplete(SwapAndReset);
    }

    private void SwapAndReset()
    {
        Sprite temp = currentImage.sprite;
        currentImage.sprite = nextImage.sprite;
        nextImage.sprite = temp;

        ResetPositions();
    }
}
