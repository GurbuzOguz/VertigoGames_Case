using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelTrackController : MonoBehaviour
{
    [Header("UI References (Auto)")]
    [SerializeField] private RectTransform ui_level_track_parent;      
    [SerializeField] private RectTransform ui_level_current_border;   
    [SerializeField] private GameObject ui_level_icon_prefab;          

    [Header("Settings")]
    [SerializeField] private int totalLevels = 50;       
    [SerializeField] private float baseSpacing = 120f;     
    [SerializeField] private float moveDuration = 0.35f;   
    [SerializeField] private bool centerOnStart = true;    

    [Header("Colors")]
    [SerializeField] private Color levelNormalColor = Color.white;
    [SerializeField] private Color levelSafeColor   = new Color(0.4f, 1f, 0.4f); 
    [SerializeField] private Color levelSuperColor  = new Color(1f, 0.85f, 0.2f); 

    private readonly List<LevelIconView> _icons = new();
    private bool _initialized = false;

    private void OnValidate()
    {
        if (ui_level_track_parent == null)
        {
            ui_level_track_parent = GetComponent<RectTransform>();
        }

        if (ui_level_current_border == null)
        {
            var borderTransform = transform.root.Find("ui_level_current_border");
            if (borderTransform != null)
                ui_level_current_border = borderTransform.GetComponent<RectTransform>();
        }
    }

    
    private void Start()
    {
        InitializeIfNeeded();

        if (centerOnStart)
        {
            CenterLevelInstant(1);
        }
    }

    private void OnEnable()
    {
        WheelEvents.OnLevelReset        += OnLevelReset;
        WheelEvents.OnLevelNumberChanged += OnLevelNumberChanged;
    }

    private void OnDisable()
    {
        WheelEvents.OnLevelReset        -= OnLevelReset;
        WheelEvents.OnLevelNumberChanged -= OnLevelNumberChanged;
    }

    private void InitializeIfNeeded()
    {
        if (_initialized) return;
        _initialized = true;

        BuildIcons();
        PositionIcons();
        UpdateAllIcons();
    }

    private void BuildIcons()
    {
        _icons.Clear();

        for (int i = ui_level_track_parent.childCount - 1; i >= 0; i--)
        {
            Destroy(ui_level_track_parent.GetChild(i).gameObject);
        }

        for (int i = 1; i <= totalLevels; i++)
        {
            var go = Instantiate(ui_level_icon_prefab, ui_level_track_parent);
            var rect = go.GetComponent<RectTransform>();

            var text = go.GetComponentInChildren<TMP_Text>(true);
            if (text == null)
            {
                Debug.LogError("[LevelTrackController] Level icon prefab'ta TMP_Text bulunamadı. " +
                               "Lütfen child'a 'level_value' isimli TMP ekleyin.", go);
            }

            var iconView = new LevelIconView(rect, text);
            _icons.Add(iconView);
        }
    }

    private void PositionIcons()
    {
        float spacing = baseSpacing;

        for (int i = 0; i < _icons.Count; i++)
        {
            var rect = _icons[i].Rect;
            rect.anchoredPosition = new Vector2(i * spacing, 0f);
        }
    }

    private void UpdateAllIcons()
    {
        for (int i = 0; i < _icons.Count; i++)
        {
            int levelNumber = i + 1;
            Color levelColor = GetColorForLevel(levelNumber);

            _icons[i].SetLevel(levelNumber, levelColor);
        }
    }

    private Color GetColorForLevel(int level)
    {
        if (level == 1)
            return levelSafeColor;

        if (level % 30 == 0)
            return levelSuperColor;

        if (level % 5 == 0)
            return levelSafeColor;

        return levelNormalColor;
    }

    
    private void CenterLevelInstant(int level)
    {
        CenterLevel(level, true);
    }

    private void CenterLevelSmooth(int level)
    {
        CenterLevel(level, false);
    }

    private void CenterLevel(int level, bool instant)
    {
        if (_icons.Count == 0) return;
        if (level < 1 || level > _icons.Count) return;

        RectTransform targetRect = _icons[level - 1].Rect;

        float diffX = ui_level_current_border.position.x - targetRect.position.x;
        Vector3 targetWorldPos = ui_level_track_parent.position + new Vector3(diffX, 0f, 0f);

        if (instant)
        {
            ui_level_track_parent.position = targetWorldPos;
        }
        else
        {
            ui_level_track_parent
                .DOMoveX(targetWorldPos.x, moveDuration)
                .SetEase(Ease.OutCubic);
        }
    }

    private void OnLevelNumberChanged(int level)
    {
        InitializeIfNeeded();

        UpdateAllIcons();
        CenterLevelSmooth(level);
    }

    private void OnLevelReset()
    {
        InitializeIfNeeded();

        UpdateAllIcons();
        CenterLevelInstant(1);
    }

    private class LevelIconView
    {
        public RectTransform Rect { get; }
        private readonly TMP_Text _levelText;

        public LevelIconView(RectTransform rect, TMP_Text levelText)
        {
            Rect = rect;
            _levelText = levelText;
        }

        public void SetLevel(int levelNumber, Color color)
        {
            if (_levelText == null) return;

            _levelText.text = levelNumber.ToString();
            _levelText.color = color;
        }
    }
}