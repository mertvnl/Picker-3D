using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Database/New Collectable", fileName = "New Collectable")]
public class CollectableLevelObjectData : LevelObjectData
{
    [field: SerializeField] public CollectableType CollectableType { get; set; }
}
