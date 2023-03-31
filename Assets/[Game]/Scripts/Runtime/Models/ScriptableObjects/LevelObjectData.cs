using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Database/New Level Object", fileName = "New Level Object")]
public class LevelObjectData : ScriptableObject
{
    [field: SerializeField] public GameObject Prefab { get; set; }
    [field: SerializeField] public float ObjectSize { get; set; }
}
