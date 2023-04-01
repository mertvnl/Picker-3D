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
    private static CollectableType collectableType;
    private static int requiredDepositCount = 10;
    private static List<LevelObjectData> levelObjectsData = new List<LevelObjectData>();
    private static GameObject levelRoot;
    private static float offset;
    private static bool isCollectableSpawned;

    private const int MIN_DEPOSIT = 1;
    private const int MAX_DEPOSIT = 100;
    private const string LEVEL_DATA_PATH = "Assets/[Game]/Data/Level/";
    private const string LEVEL_PREFAB_PATH = "Assets/[Game]/Prefabs/Level/";

    [MenuItem("Picker3D/Level Creator")]
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
            GUILayout.Label("Collectable Type to Spawn");

            collectableType = (CollectableType)EditorGUILayout.EnumPopup(collectableType);
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

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    DestroyLevelRoot();
                    ResetSettings();
                }
            }

            if (GUILayout.Button("Reset"))
            {
                DestroyLevelRoot();
                ResetSettings();
            }
        }

        DrawMessageBoxes();

        GUILayout.EndArea();
    }

    private void OnSceneGUI(SceneView view)
    {
        if (levelObjectType != LevelObjectType.Collectable)
            return;

        HandleCollectables();
    }

    private void HandleCollectables()
    {
        UnityEngine.Event e = UnityEngine.Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
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

        if (e.type == EventType.MouseDown && e.button == 1)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            int layer = 1 << 8;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                foreach (Collider col in Physics.OverlapSphere(hit.point, 0.1f))
                {
                    if (col.TryGetComponent(out ICollectable collectable))
                    {
                        DestroyImmediate(collectable.transform.parent.gameObject);
                    }
                }
            }
        }
    }

    private static void ResetSettings()
    {
        levelObjectType = LevelObjectType.Platform;
        requiredDepositCount = 10;
        levelObjectsData = new List<LevelObjectData>();
        levelRoot = null;
        offset = 0f;
        isCollectableSpawned = false;
    }

    private static void DrawMessageBoxes()
    {
        if (levelRoot != null)
        {
            if (!isCollectableSpawned)
                EditorGUILayout.HelpBox("You should spawn some collectables.", MessageType.Warning);

            if (!levelObjectsData.Find(x => x.Type == LevelObjectType.Finish))
                EditorGUILayout.HelpBox("In order to create the level you need to spawn ONE 'Finish' level object type at the end of the level.", MessageType.Warning);

            if (levelObjectType == LevelObjectType.Collectable)
            {
                EditorGUILayout.HelpBox("You can spawn collectables by left clicking to platform.", MessageType.Info);
                EditorGUILayout.HelpBox("You can delete spawned collectables by RIGHT clicking.", MessageType.Info);
            }
        }
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
        if (type == LevelObjectType.Collectable)
            return Resources.Load<LevelObjectData>($"LevelObjects/Collectables/{collectableType}");
        else
            return Resources.Load<LevelObjectData>($"LevelObjects/{type}");
    }
}