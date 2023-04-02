using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Database/New Level Database", fileName = "LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    [field: SerializeField] public List<LevelData> Levels { get; private set; } = new List<LevelData>();

    public LevelData GetLevelDataByIndex(int index)
    {
        return Levels[index];
    }

    public void ClearEmptyLevels()
    {
        Levels = Levels.Where(x => x != null).ToList();
        OrderLevels();
    }

    public void AddNewLevel(LevelData levelData)
    {
        if (Levels.Contains(levelData))
            return;

        Levels.Add(levelData);

        OrderLevels();
    }

    private void OrderLevels()
    {
        Levels = Levels.OrderBy(x => int.Parse(x.name.Replace("Level ", ""))).ToList();
    }
}
