using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Database/New Deposit Level Object", fileName = "New Deposit Level Object")]
public class DepositLevelObjectData : LevelObjectData
{
    [field: SerializeField] public int RequiredDepositCount { get; set; } = 0;
}
