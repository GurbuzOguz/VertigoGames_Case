using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WheelLevelDataBase))]
public class WheelLevelDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WheelLevelDataBase db = (WheelLevelDataBase)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("LEVEL TOOLS", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New Level"))
        {
            Undo.RecordObject(db, "Add New Level");
            var method = db.GetType().GetMethod("AddNewLevel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(db, null);
            EditorUtility.SetDirty(db);
        }

        EditorGUILayout.Space(10);

        db.setLevelCount = EditorGUILayout.IntField("Total Level Count", db.setLevelCount);

        if (GUILayout.Button("Set All Levels"))
        {
            Undo.RecordObject(db, "Set All Levels");
            SetAllLevels(db);
            
            PlayerPrefs.SetInt("TotalLevels", db.levels.Count);
            PlayerPrefs.Save();
            
            EditorUtility.SetDirty(db);
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("CURRENT LEVEL TOOLS", EditorStyles.boldLabel);

        db.currentLevelIndex = EditorGUILayout.IntField("Current Level Index", db.currentLevelIndex);

        if (GUILayout.Button("Set Current Level"))
        {
            Undo.RecordObject(db, "Set Current Level");

            int index = Mathf.Clamp(db.currentLevelIndex, 1, db.levels.Count);

            PlayerPrefs.SetInt("CurrentLevelIndex", index - 1);
            PlayerPrefs.Save();
            Debug.Log($"[EDITOR] CurrentLevelIndex => {index - 1} olarak kaydedildi.");

            EditorUtility.SetDirty(db);
        }
    }

    private void SetAllLevels(WheelLevelDataBase db)
    {
        int targetCount = db.setLevelCount;
        if (targetCount < 0) targetCount = 0;

        while (db.levels.Count > targetCount)
            db.levels.RemoveAt(db.levels.Count - 1);

        while (db.levels.Count < targetCount)
        {
            var method = db.GetType().GetMethod("AddNewLevel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(db, null);
        }
    }
}
