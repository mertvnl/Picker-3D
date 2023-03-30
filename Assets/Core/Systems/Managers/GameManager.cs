using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool isLevelCompleted;

    public Event<bool> OnLevelCompleted = new Event<bool>();

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        DOTween.SetTweensCapacity(500, 500);
    }

    private void OnEnable()
    {
        LevelSystem.Instance.OnLevelLoaded.AddListener(ResetLevelCompletionStatus);
        Events.OnLevelSuccess.AddListener(ResetLevelCompletionStatus);
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnLevelLoaded.RemoveListener(ResetLevelCompletionStatus);
        Events.OnLevelSuccess.RemoveListener(ResetLevelCompletionStatus);
    }

    [Button]
    public void CompleteLevel(bool isSuccess)
    {
        if (isLevelCompleted) return;

        isLevelCompleted = true;

        OnLevelCompleted.Invoke(isSuccess);
        LevelSystem.Instance.FinishLevel();
        if (isSuccess)
            SaveLoadSystem.SetInt("FakeLevel", SaveLoadSystem.GetInt("FakeLevel", 1) + 1);
    }

    private void ResetLevelCompletionStatus()
    {
        isLevelCompleted = false;
    }
}
