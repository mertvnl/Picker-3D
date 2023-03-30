using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Database/New Level Database", fileName = "LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    public LevelData[] levels;
}
