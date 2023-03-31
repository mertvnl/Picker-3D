using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum LevelObjectType
{
    Platform,
    Deposit,
    Finish,
    Collectable
}

public class LevelEditor : EditorWindow
{
    public static LevelObjectType LevelObjectType;
    public static int RequiredDepositCount = 10;
    public static LevelData LevelData;
    public static List<LevelObjectData> LevelObjectsData = new List<LevelObjectData>();
    public static GameObject LevelRoot;

    private static LevelDatabase levelDatabase;
    public static LevelDatabase LevelDatabase => levelDatabase == null ? Resources.Load<LevelDatabase>("LevelDatabase") : levelDatabase;

    private const int MIN_DEPOSIT = 1;
    private const int MAX_DEPOSIT = 100;
    private const string LEVEL_DATA_PATH = "Assets/[Game]/Data/Level/";
    private const string LEVEL_PREFAB_PATH = "Assets/[Game]/Prefabs/Level/";

    [MenuItem("Picker3D/Level Editor")]
    private static void OpenWindow()
    {
        LevelEditor levelEditor = (LevelEditor)GetWindow(typeof(LevelEditor));
        levelEditor.minSize = new Vector2(600, 300);
        ResetSettings();
        levelEditor.Show();
    }

    private void OnGUI()
    {
        DrawLevelSettings();
    }

    private void OnDestroy()
    {
        if (LevelRoot != null)
            DestroyImmediate(LevelRoot);
    }

    private void DrawLevelSettings()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        GUILayout.Label("Level Object Type To Spawn");
        GUILayout.Space(10);

        LevelObjectType = (LevelObjectType)EditorGUILayout.EnumPopup(LevelObjectType);

        if (LevelObjectType == LevelObjectType.Deposit)
        {
            GUILayout.Label($"Deposit Reqiurement ({MIN_DEPOSIT}-{MAX_DEPOSIT})");
            GUILayout.Label(RequiredDepositCount.ToString());
            RequiredDepositCount = (int)GUILayout.HorizontalSlider(RequiredDepositCount, MIN_DEPOSIT, MAX_DEPOSIT);
        }

        GUILayout.Space(25);

        if (GUILayout.Button("Spawn"))
        {
            if (LevelRoot == null)
                LevelRoot = new GameObject(GetLevelName());


        }

        if (LevelRoot != null)
        {
            if (GUILayout.Button("Create Level"))
            {
                string prefabPath = LEVEL_PREFAB_PATH + GetLevelName() + ".prefab";
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(LevelRoot, prefabPath);

                LevelData levelData = CreateInstance<LevelData>();
                levelData.SetLevelObjects(LevelObjectsData);
                levelData.SetLevelPrefab(prefab); 

                string levelPath = LEVEL_DATA_PATH + GetLevelName() + ".asset";
                AssetDatabase.CreateAsset(levelData, levelPath);
            }
        }



        GUILayout.EndArea();
    }

    private static void ResetSettings()
    {
        LevelObjectType = LevelObjectType.Platform;
        RequiredDepositCount = 10;
        LevelObjectsData = new List<LevelObjectData>();
        LevelRoot = null;
    }

    private string GetLevelName()
    {
        return $"Level {LevelDatabase.levels.Length + 1}";
    }
}