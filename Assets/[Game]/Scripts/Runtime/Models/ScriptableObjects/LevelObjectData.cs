using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Database/New Level Object", fileName = "New Level Object")]
public class LevelObjectData : ScriptableObject
{
    [field: SerializeField] public LevelObjectType Type { get; set; }
    [field: SerializeField] public GameObject Prefab { get; set; }
    [field: SerializeField] public float ObjectSize { get; set; }
}

public enum LevelObjectType
{
    Platform,
    Deposit,
    Finish,
    Collectable
}

public enum CollectableType
{
    Sphere,
    Cube,
    Capsule,
    Cylinder
}