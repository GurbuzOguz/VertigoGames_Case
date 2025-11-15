using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class LevelTrackController : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private RectTransform ui_parent; // Kayan konteyner

    [SerializeField] private RectTransform ui_currentBorder; // Sabit border
    [SerializeField] private GameObject levelIconPrefab;

    [Header("Settings")] [SerializeField] private int totalLevels = 50;
    [SerializeField] private float spacing = 120f; // ikonlar arası mesafe
    [SerializeField] private float moveDuration = 0.35f; // kayma süresi

    [Header("Colors")] [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color greenColor = new Color(0.4f, 1f, 0.4f);
    [SerializeField] private Color goldColor = new Color(1f, 0.85f, 0.2f);

    private List<LevelIcon> icons = new();


    // ============================================================
    // INIT
    // ============================================================
    private void Start()
    {
        BuildIcons();
        PositionIcons();
        UpdateAllIcons();
        CenterInstant(1);
    }

    private void OnEnable()
    {
        WheelEvents.OnLevelReset += OnLevelReset;
        WheelEvents.OnLevelNumberChanged += CenterOnLevel;
    }

    private void OnDisable()
    {
        WheelEvents.OnLevelNumberChanged -= CenterOnLevel;
        WheelEvents.OnLevelReset -= OnLevelReset;
    }

    // ============================================================
    // ICON BUILD
    // ============================================================
    private void BuildIcons()
    {
        icons.Clear();

        for (int i = 1; i <= totalLevels; i++)
        {
            var go = Instantiate(levelIconPrefab, ui_parent);
            var rect = go.GetComponent<RectTransform>();
            var icon = new LevelIcon(rect);

            icons.Add(icon);
        }
    }


    private void PositionIcons()
    {
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].rect.anchoredPosition = new Vector2(i * spacing, 0);
        }
    }


    // ============================================================
    // UPDATE COLORS + TEXT
    // ============================================================
    private void UpdateAllIcons()
    {
        for (int i = 0; i < icons.Count; i++)
        {
            int level = i + 1;

            icons[i].SetLevelNumber(level, normalColor, greenColor, goldColor);
        }
    }


    // ============================================================
    // CENTERING
    // ============================================================
    private void CenterInstant(int level)
    {
        Center(level, true);
    }

    private void CenterOnLevel(int level)
    {
        Center(level, false);
    }

    private void Center(int level, bool instant)
    {
        if (level < 1 || level > icons.Count) return;

        RectTransform target = icons[level - 1].rect;

        // dünya pozisyonları arasındaki fark
        float diff = ui_currentBorder.position.x - target.position.x;

        Vector3 newWorldPos = ui_parent.position + new Vector3(diff, 0, 0);

        if (instant)
        {
            ui_parent.position = newWorldPos;
        }
        else
        {
            ui_parent.DOMoveX(newWorldPos.x, moveDuration)
                .SetEase(Ease.OutCubic);
        }
    }


    // ============================================================
    // LEVEL ICON CLASS
    // ============================================================
    private class LevelIcon
    {
        public RectTransform rect;
        private TMP_Text text;

        public LevelIcon(RectTransform r)
        {
            rect = r;
            text = r.GetComponentInChildren<TMP_Text>();
        }

        public void SetLevelNumber(int lvl, Color normal, Color green, Color gold)
        {
            text.text = lvl.ToString();

            // ⭐ 1 numara özel durum
            if (lvl == 1)
            {
                text.color = green;
                return;
            }

            // ⭐ 30, 60, 90...
            if (lvl % 30 == 0)
            {
                text.color = gold;
                return;
            }

            // ⭐ 5, 10, 15, 20...
            if (lvl % 5 == 0)
            {
                text.color = green;
                return;
            }

            // ⭐ Diğerleri
            text.color = normal;
        }
    }

    private void OnLevelReset()
    {
        // Level 1’e dön
        UpdateAllIcons(); // 1 → 50 textleri ve renkleri yeniden bas
        Center(1, false); // yumuşak animasyon ile merkeze gelsin
    }
}