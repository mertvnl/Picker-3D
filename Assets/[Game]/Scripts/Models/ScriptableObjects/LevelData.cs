using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Database/New Level", fileName = "New Level")]
public class LevelData : ScriptableObject
{
    public GameObject Prefab;
    public float LevelLength;
}