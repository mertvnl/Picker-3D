using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Database/New Level", fileName = "New Level")]
public class LevelData : ScriptableObject
{
    [field: SerializeField, ReadOnly] public List<LevelObjectData> LevelObjects { get; private set; } = new List<LevelObjectData>();
    [field: SerializeField] public GameObject LevelPrefab { get; private set; }
    [ShowInInspector] public float LevelLength => GetLevelSize();

    private float GetLevelSize()
    {
        if (LevelObjects.Count == 0)
            return 0f;

        float size = 0f;

        LevelObjects.ForEach(x => size += x.ObjectSize);

        return size;
    }

    public void SetLevelObjects(List<LevelObjectData> levelObjects)
    {
        LevelObjects = levelObjects;
    }

    public void SetLevelPrefab(GameObject prefab)
    {
        LevelPrefab = prefab;
    }
}