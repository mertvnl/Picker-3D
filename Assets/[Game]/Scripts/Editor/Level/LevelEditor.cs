using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    private static LevelDatabase levelDatabase;
    public static LevelDatabase LevelDatabase => levelDatabase == null ? Resources.Load<LevelDatabase>("LevelDatabase") : levelDatabase;

    private static LevelObjectType levelObjectType;
    private static int requiredDepositCount = 10;
    private static List<LevelObjectData> levelObjectsData = new List<LevelObjectData>();
    private static GameObject levelRoot;
    private static float offset;
    private static bool isCollectableSpawned;

    private const int MIN_DEPOSIT = 1;
    private const int MAX_DEPOSIT = 100;
    private const string LEVEL_DATA_PATH = "Assets/[Game]/Data/Level/";
    private const string LEVEL_PREFAB_PATH = "Assets/[Game]/Prefabs/Level/";

    [MenuItem("Picker3D/Level Editor")]
    private static void OpenWindow()
    {
        LevelEditor levelEditor = (LevelEditor)GetWindow(typeof(LevelEditor));
        levelEditor.minSize = new Vector2(320, 500);
        ResetSettings();
        levelEditor.Show();
    }

    private void OnGUI()
    {
        DrawLevelSettings();
    }

    private void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    private void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

    private void OnDestroy()
    {
        DestroyLevelRoot();
    }

    private void DrawLevelSettings()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        GUILayout.Label("Level Object Type To Spawn");
        GUILayout.Space(10);

        levelObjectType = (LevelObjectType)EditorGUILayout.EnumPopup(levelObjectType);

        if (levelObjectType == LevelObjectType.Deposit)
        {
            GUILayout.Label($"Deposit Reqiurement ({MIN_DEPOSIT}-{MAX_DEPOSIT})");
            GUILayout.Label(requiredDepositCount.ToString());
            requiredDepositCount = (int)GUILayout.HorizontalSlider(requiredDepositCount, MIN_DEPOSIT, MAX_DEPOSIT);
        }

        GUILayout.Space(25);

        if (levelObjectType != LevelObjectType.Collectable)
        {
            if (GUILayout.Button("Spawn"))
            {
                if (levelRoot == null)
                {
                    levelRoot = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("LevelBase")) as GameObject;
                    levelRoot.name = GetLevelName();
                }

                LevelObjectData levelObjectData = GetLevelObjectByType(levelObjectType);

                GameObject go = PrefabUtility.InstantiatePrefab(levelObjectData.Prefab, levelRoot.transform) as GameObject;
                go.transform.position = Vector3.forward * offset;
                offset += levelObjectData.ObjectSize;

                if (levelObjectType == LevelObjectType.Deposit)
                    go.GetComponentInChildren<IDeposit>().ReqiuredDepositCount = requiredDepositCount;

                levelObjectsData.Add(levelObjectData);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("You can spawn collectables by left clicking to platform.", MessageType.Info);
            EditorGUILayout.HelpBox("You can delete spawned collectables by SHIFT + left click", MessageType.Info);
        }

        if (levelRoot != null)
        {
            if (levelObjectsData.Find(x => x.Type == LevelObjectType.Finish))
            {
                if (GUILayout.Button("Create Level"))
                {
                    string prefabPath = LEVEL_PREFAB_PATH + GetLevelName() + ".prefab";
                    GameObject prefab = PrefabUtility.SaveAsPrefabAsset(levelRoot, prefabPath);

                    LevelData levelData = CreateInstance<LevelData>();
                    levelData.SetLevelObjects(levelObjectsData);
                    levelData.SetLevelPrefab(prefab);

                    string levelPath = LEVEL_DATA_PATH + GetLevelName() + ".asset";
                    AssetDatabase.CreateAsset(levelData, levelPath);

                    LevelDatabase.AddNewLevel(levelData);

                    DestroyLevelRoot();
                    ResetSettings();
                }
            }
            else
                EditorGUILayout.HelpBox("In order to create the level you need to spawn ONE 'Finish' level object type at the end of the level.", MessageType.Warning);

            if (!isCollectableSpawned)
                EditorGUILayout.HelpBox("You should spawn some collectables.", MessageType.Warning);

            if (GUILayout.Button("Reset"))
            {
                DestroyLevelRoot();
                ResetSettings();
            }
        }

        GUILayout.EndArea();
    }

    private void OnSceneGUI(SceneView view)
    {
        if (levelObjectType != LevelObjectType.Collectable)
            return;

        SpawnCollectable();
    }

    private void SpawnCollectable()
    {
        if (UnityEngine.Event.current.type == EventType.MouseDown)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(UnityEngine.Event.current.mousePosition);
            RaycastHit hit;
            int layer = 1 << 8;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                if (levelRoot == null)
                    levelRoot = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("LevelBase")) as GameObject;

                LevelObjectData levelObjectData = GetLevelObjectByType(levelObjectType);

                Transform parent = levelRoot.GetComponentInChildren<CollectableHolder>().transform;

                GameObject go = PrefabUtility.InstantiatePrefab(levelObjectData.Prefab, parent ?? levelRoot.transform) as GameObject;
                go.transform.position = hit.point;

                isCollectableSpawned = true;
            }
        }

        UnityEngine.Event.current.Use();
    }

    private static void ResetSettings()
    {
        levelObjectType = LevelObjectType.Platform;
        requiredDepositCount = 10;
        levelObjectsData = new List<LevelObjectData>();
        levelRoot = null;
        offset = 0f;
    }

    private static void DestroyLevelRoot()
    {
        if (levelRoot != null)
            DestroyImmediate(levelRoot);
    }

    private string GetLevelName()
    {
        LevelDatabase.ClearEmptyLevels();

        return $"Level {LevelDatabase.Levels.Count + 1}";
    }

    private LevelObjectData GetLevelObjectByType(LevelObjectType type)
    {
        return Resources.Load<LevelObjectData>($"LevelObjects/{type}");
    }
}