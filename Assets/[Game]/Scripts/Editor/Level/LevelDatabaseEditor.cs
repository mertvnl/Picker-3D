using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelDatabaseEditor : EditorWindow
{
    private static LevelDatabase levelDatabase;
    public static LevelDatabase LevelDatabase => levelDatabase == null ? Resources.Load<LevelDatabase>("LevelDatabase") : levelDatabase;

    private static int levelDataIndex;
    private static LevelData levelData => LevelDatabase.Levels[levelDataIndex];
    private static GameObject previewInstance;

    private const string LEVEL_DATA_PATH = "Assets/[Game]/Data/Level/";
    private const string LEVEL_PREFAB_PATH = "Assets/[Game]/Prefabs/Level/";


    [MenuItem("Picker3D/Level Database Editor")]
    private static void OpenWindow()
    {
        LevelDatabaseEditor levelEditor = (LevelDatabaseEditor)GetWindow(typeof(LevelDatabaseEditor));
        levelEditor.minSize = new Vector2(320, 500);
        levelEditor.Show();
    }

    private void OnDestroy()
    {
        Reset();
    }

    private void OnGUI()
    {
        DrawLevelDatabaseSettings();
    }

    private void DrawLevelDatabaseSettings()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (LevelDatabase.Levels.Count == 0)
        {
            EditorGUILayout.HelpBox("There is NO level to edit.", MessageType.Error);
            GUILayout.EndArea();
            return;
        }

        levelDataIndex = EditorGUILayout.Popup("Selected Level To Edit ", levelDataIndex, GetLevels());

        if (GUILayout.Button("Preview Level"))
        {
            if (previewInstance != null)
                DestroyImmediate(previewInstance);
            previewInstance = PrefabUtility.InstantiatePrefab(levelData.LevelPrefab) as GameObject;
        }

        if (GUILayout.Button("Delete Level"))
        {
            DeleteLevelFromDatabase();
        }

        GUILayout.EndArea();
    }

    private void Reset()
    {
        if (previewInstance != null)
            DestroyImmediate(previewInstance);

        levelDataIndex = 0;
    }

    private void DeleteLevelFromDatabase()
    {
        string levelName = levelData.name;
        AssetDatabase.DeleteAsset(LEVEL_PREFAB_PATH + levelName + ".prefab");
        AssetDatabase.DeleteAsset(LEVEL_DATA_PATH + levelName + ".asset");

        UpdateLevelDatabase();


        Reset();
    }

    private void UpdateLevelDatabase()
    {
        LevelDatabase.ClearEmptyLevels();

        for (int i = 0; i < LevelDatabase.Levels.Count; i++)
        {
            AssetDatabase.RenameAsset(LEVEL_PREFAB_PATH + LevelDatabase.Levels[i].name + ".prefab", $"Level {i + 1}");
            AssetDatabase.RenameAsset(LEVEL_DATA_PATH + LevelDatabase.Levels[i].name + ".asset", $"Level {i + 1}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private string[] GetLevels()
    {
        LevelDatabase.ClearEmptyLevels();

        List<string> levelsList = new List<string>();

        foreach (var level in LevelDatabase.Levels)
            levelsList.Add(level.name);

        return levelsList.ToArray();
    }
}
